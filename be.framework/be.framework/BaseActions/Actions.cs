using RestSharp;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AventStack.ExtentReports;
using Backend.Framework.Models;
using Backend.Framework.Utilities;

namespace Backend.Framework.BaseActions
{
    public class Actions
    {
        private static RestClient restClient;
        public static string url;
        private ExtentTest test;

        public Actions(ExtentTest test)
        {
            this.test = test;
        }
        public static RestClient NewRestClient()
        {
            restClient = new();
            return restClient;
        }

        public RestRequest NewGetRestRequest(string path)
        {
            RestRequest restRequest = new(path, Method.Get);
            return restRequest;
        }

        public RestRequest AddQueryParameterToGetRequest(string parameterName, string parameterValue, RestRequest restRequest)
        {
            test.Log(Status.Info, "Adding query parameters to GET request");

            restRequest.AddParameter(parameterName, parameterValue, ParameterType.QueryString);

            return restRequest;
        }

        public async Task<string> ObtainJWToken(string url)
        {
            test.Log(Status.Info, "Obtaining JSon web token for authentication");

            var request = new RestRequest(url, Method.Post);
            
            request.AddHeader("Content-Type", "application/json");
            request.AddStringBody(Properties.credentials, DataFormat.Json);

            var response = await restClient.ExecuteAsync(request);

            TokenModel token = JsonConvert.DeserializeObject<TokenModel>(response.Content.ToString());

            string jwtToken = token.Token;

            test.Log(Status.Info, "Received JSon web token");

            return jwtToken;
        }

        public async Task<RestResponse> GetRequest(string url)
        {
            var request = new RestRequest(url, Method.Get);

            var response = await restClient.ExecuteAsync(request);

            return response;
        }

        public async Task<RestResponse> PostResquest(string url)
        {
            var request = new RestRequest(url, Method.Post);

            var response = await restClient.ExecuteAsync(request);

            return response;
        }

    }
}
