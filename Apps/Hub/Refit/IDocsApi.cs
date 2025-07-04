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
}