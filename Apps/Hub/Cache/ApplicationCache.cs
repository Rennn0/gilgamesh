using System.Runtime.CompilerServices;
using Enyim.Caching.Memcached;
using MessagePack;
using Prometheus;

namespace Hub.Cache;

/// <summary>
/// <exception cref="CacheNotFoundException"></exception>
/// </summary>
public sealed class ApplicationCache : IApplicationCache
{
    public class CacheNotFoundException : Exception
    {
        public CacheNotFoundException(string key)
            : base(key) { }
    }

    private readonly MemcachedCluster _cluster;
    private const int BytesLimit = 10240; // 10 KB
    private static readonly Counter _fromCacheCounter = Metrics.CreateCounter(
        "hub_application_cache_from_cache",
        "Number of times data was retrieved from cache"
    );
    private static readonly Histogram _cacheSetDuration = Metrics.CreateHistogram(
        "hub_application_cache_set_duration_seconds",
        "Duration of cache set operations",
        new HistogramConfiguration { Buckets = Histogram.ExponentialBuckets(0.001, 2, 10) }
    );
    private static readonly Histogram _cacheGetDuration = Metrics.CreateHistogram(
        "hub_application_cache_get_duration_seconds",
        "Duration of cache get operations",
        new HistogramConfiguration { Buckets = Histogram.ExponentialBuckets(0.001, 2, 10) }
    );
    private static readonly Gauge _cacheSize = Metrics.CreateGauge(
        "hub_application_cache_size",
        "Current size of the application cache in bytes"
    );

    /// <summary>
    /// </summary>
    /// <param name="memcachedCluster">localhost:11211</param>
    public ApplicationCache(string memcachedCluster)
    {
        _cluster = new MemcachedCluster(memcachedCluster);
        _cluster.Start();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        using (_cacheGetDuration.NewTimer())
        {
            IMemcachedClient client = _cluster.GetClient();
            byte[]? bytes = await client.GetAsync<byte[]?>(key);
            if (bytes is null)
            {
                return default;
            }
            _fromCacheCounter.Inc();
            return Deserialize<T>(bytes);
        }
    }

    public Task<bool> SetAsync(string key, object data) =>
        SetAsync(key, GetBytes(data, false), TimeSpan.FromDays(7));

    public Task<bool> SetAsync(string key, object data, TimeSpan expiration) =>
        SetAsync(key, GetBytes(data, false), expiration);

    public Task<bool> SetAsync(string key, object data, bool replace) =>
        SetAsync(key, GetBytes(data, false), replace, TimeSpan.FromDays(7));

    public Task<bool> SetAsync(string key, object data, bool replace, TimeSpan expiration) =>
        SetAsync(key, GetBytes(data, false), replace, expiration);

    public Task<bool> DeleteAsync(string key)
    {
        IMemcachedClient client = _cluster.GetClient();
        return client.DeleteAsync(key);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private byte[] GetBytes(object data, bool compress) =>
        compress ? Compress(data) : Serialize(data);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Task<bool> SetAsync(string key, byte[] compressed, TimeSpan expiration)
    {
        using (_cacheSetDuration.NewTimer())
        {
            IMemcachedClient client = _cluster.GetClient();
            _cacheSize.Inc(compressed.Length);
            return client.StoreAsync(StoreMode.Set, key, compressed, expiration);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Task<bool> SetAsync(string key, byte[] compressed, bool replace, TimeSpan expiration)
    {
        using (_cacheSetDuration.NewTimer())
        {
            IMemcachedClient client = _cluster.GetClient();
            _cacheSize.Inc(compressed.Length);
            return client.StoreAsync(
                replace ? StoreMode.Replace : StoreMode.Add,
                key,
                compressed,
                expiration
            );
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T Deserialize<T>(in byte[] bytes) =>
        bytes.Length >= BytesLimit
            ? LZ4MessagePackSerializer.Deserialize<T>(bytes)
            : MessagePackSerializer.Deserialize<T>(bytes);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte[] Serialize(in object obj) => MessagePackSerializer.Serialize(obj);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte[] Compress(in object obj) => LZ4MessagePackSerializer.Serialize(obj);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte[] Compress(in string str) =>
        str.Length >= BytesLimit
            ? LZ4MessagePackSerializer.Serialize(str)
            : MessagePackSerializer.Serialize(str);
}
