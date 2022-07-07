using AventStack.ExtentReports;
using Backend.Framework.Models;
using Backend.Framework.Utilities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Framework.BaseActions
{
    public class ExampleActions
    {
        private static RestClient restClient = Actions.NewRestClient();
        public static string url;
        private ExtentTest test;

        public ExampleActions(ExtentTest test)
        {
            this.test = test;
        }

        public async Task<int> GetBookingId(string url)
        {
            test.Log(Status.Info, "Receiving Booking ID");

            RestRequest restResquest = new(Properties.herokuBaseUrl + Properties.herokuBookings);

            var restResponse = await restClient.ExecuteAsync(restResquest);

            var bookingIDsModels = JsonConvert.DeserializeObject<List<AllBookingIDsModel>>(restResponse.Content.ToString());

            int bookingId = bookingIDsModels[0].Id;

            test.Log(Status.Info, "Booking ID received");

            return bookingId;
        }

        public async Task<string> GetBookingFirstName(string url)
        {
            test.Log(Status.Info, "Receiving first tname");

            int bookingId = await GetBookingId(Properties.herokuBookings + Properties.herokuBookings);

            RestRequest restResquest = new(Properties.herokuBaseUrl + Properties.herokuBookings + bookingId);
            restResquest.AddHeader("Accept", "application/json");

            var restResponse = await restClient.ExecuteAsync(restResquest);

            NewBookingModel responseString = JsonConvert.DeserializeObject<NewBookingModel>(restResponse.Content);

            test.Log(Status.Info, "First name received");

            return responseString.Firstname;
        }
    }
}
