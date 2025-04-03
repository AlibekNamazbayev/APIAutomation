using ApiTest.Core;
using RestSharp;
<<<<<<< HEAD
=======
using ApiTest.Business.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
>>>>>>> 5376926 (fixed codes by comments)

namespace ApiTest.Clients
{
    public class JsonPlaceholderClient : BaseApiClient
    {
        public JsonPlaceholderClient() : base("https://jsonplaceholder.typicode.com") { }

<<<<<<< HEAD
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
=======
        public async Task<RestResponse<List<UserModel>>> GetUsersAsync()
        {
            var request = new RestRequest("/users", Method.Get);
            return await ExecuteRequestAsync<List<UserModel>>(request); 
        }

        public async Task<RestResponse<UserModel>> CreateUserAsync(object userData)
        {
            var request = new RestRequest("/users", Method.Post);
            request.AddJsonBody(userData);
            return await ExecuteRequestAsync<UserModel>(request); 
>>>>>>> 5376926 (fixed codes by comments)
        }

        public async Task<RestResponse> ExecuteAsync(RestRequest request)
        {
<<<<<<< HEAD
            return await ExecuteRequestAsync(request);
        }
    }
}
=======
            return await ExecuteRequestAsync(request); 
        }
    }
}
>>>>>>> 5376926 (fixed codes by comments)
