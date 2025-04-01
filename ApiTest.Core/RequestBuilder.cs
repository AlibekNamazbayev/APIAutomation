using RestSharp;

namespace ApiTest.Core
{
    public class RequestBuilder
    {
        private readonly RestRequest _request;

        public RequestBuilder(string resource, Method method)
        {
            _request = new RestRequest(resource, method);
        }

        public RequestBuilder AddJsonBody(object body)
        {
            _request.AddJsonBody(body);
            return this;
        }

        public RequestBuilder AddHeader(string name, string value)
        {
            _request.AddHeader(name, value);
            return this;
        }

        public RestRequest Build()
        {
            return _request;
        }
    }
}