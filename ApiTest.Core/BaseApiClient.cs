using NLog;
using RestSharp;
using System.IO;
using System.Threading.Tasks;

namespace ApiTest.Core
{
    public abstract class BaseApiClient
    {
        protected readonly RestClient _client;
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        static BaseApiClient()
        {
            var logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            Directory.CreateDirectory(logDir);
        }

        protected BaseApiClient(string baseUrl)
        {
            _client = new RestClient(new RestClientOptions(baseUrl));
            Logger.Info($"Initialized client for {baseUrl}");
        }

<<<<<<< HEAD
=======

        protected async Task<RestResponse<T>> ExecuteRequestAsync<T>(RestRequest request)
        {
            Logger.Info($"Executing: {request.Method} {request.Resource}");
            
            var response = await _client.ExecuteAsync<T>(request);
            
            if (!response.IsSuccessful)
            {
                Logger.Error($"API Error: {response.StatusCode} - {response.Content}");
            }
            
            return response;
        }


>>>>>>> 5376926 (fixed codes by comments)
        protected async Task<RestResponse> ExecuteRequestAsync(RestRequest request)
        {
            Logger.Info($"Executing: {request.Method} {request.Resource}");
            
            var response = await _client.ExecuteAsync(request);
            
            if (!response.IsSuccessful)
            {
                Logger.Error($"API Error: {response.StatusCode} - {response.Content}");
            }
            
            return response;
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 5376926 (fixed codes by comments)
