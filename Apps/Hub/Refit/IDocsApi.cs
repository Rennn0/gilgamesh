using Refit;

namespace Hub.Refit
{
    public interface IDocsApi
    {
        [Get("/id/{**page}")]
        public Task<TResponse> GetAsync<TResponse>(string page);

        [Get("/id/{**page}")]
        public Task<Stream> GetAsync(string page);
    }

    public interface IRandomStringApi
    {
        [Get("/api/v1.0/randomstring")]
        public Task<string[]> GetAsync([Query] int min, [Query] int max, [Query] int count);
    }
}
