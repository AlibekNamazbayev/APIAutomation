using ApiTest.Core;
using RestSharp;

namespace ApiTest.Clients
{
    public class JsonPlaceholderClient : BaseApiClient
    {
        public JsonPlaceholderClient() : base("https://jsonplaceholder.typicode.com") { }

        public async Task<RestResponse> GetUsersAsync()
        {
            var request = new RestRequest("/users", Method.Get);
            return await ExecuteRequestAsync(request);
        }

        public async Task<RestResponse> CreateUserAsync(object userData)
        {
            var request = new RestRequest("/users", Method.Post);
            request.AddJsonBody(userData);
            return await ExecuteRequestAsync(request);
        }

        public async Task<RestResponse> ExecuteAsync(RestRequest request)
        {
            return await ExecuteRequestAsync(request);
        }
    }
}