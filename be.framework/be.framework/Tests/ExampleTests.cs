using NUnit.Framework;
using System;
using System.Threading.Tasks;
using RestSharp;
using FluentAssertions;
using System.Net;
using Newtonsoft.Json;
using AventStack.ExtentReports;
using System.Collections.Generic;
using Backend.Framework.Models;
using Backend.Framework.Utilities;

namespace Backend.Framework.Tests
{
    internal class ExampleTests : BaseTests
    {
        protected string jwt;

        protected int responseBookingId = 89;
        protected int newBookingId;

        [Test]
        public async Task GetAllBookingsIdsSuccessfully()
        {
            test.Log(Status.Info, "GET all booking IDs successfully");

            jwt = await actions.ObtainJWToken(Properties.authPath);

            RestRequest restRequest = actions.NewGetRestRequest(Properties.herokuBaseUrl + Properties.herokuBookings);

            test.Log(Status.Info, "Executing GET request");
            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            try
            {
                test.Log(Status.Pass, "Fetching all booking IDs");
                restResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            catch (Exception)
            {
                test.Log(Status.Fail, "Test Fail");
                throw;
            }
        }

        [Test]
        public async Task GetBookingByNameSuccessfully()
        {
            test.Log(Status.Info, "GET Booking by fistname - Using dynamically obtained firstname from another API call");

            jwt = await actions.ObtainJWToken(Properties.authPath);

            string firstname = await exampleActions.GetBookingFirstName(Properties.herokuBaseUrl + Properties.herokuBookings);

            RestRequest restRequest = actions.NewGetRestRequest(Properties.herokuBaseUrl + Properties.herokuBookings);
            actions.AddQueryParameterToGetRequest("firstname", firstname, restRequest);

            RestResponse restResponse = await restClient.ExecuteAsync(restRequest);

            var responseString = JsonConvert.DeserializeObject<List<AllBookingIDsModel>>(restResponse.Content.ToString());

            int bookingId = responseString[0].Id;

            try
            {
                test.Log(Status.Pass, "Returining booking IDs for the given firstname");
                bookingId.Should().BePositive();
            }

            catch (Exception)
            {
                test.Log(Status.Fail, "Test Fail");
                throw;
            }
        }

        [Test]
        public async Task GetBookingByIdSuccesfully()
        {
            test.Log(Status.Info, "GET booking by dynamically obtained ID");

            jwt = await actions.ObtainJWToken(Properties.authPath);

            int bookingId = await exampleActions.GetBookingId(Properties.herokuBaseUrl + Properties.herokuBookings);

            RestRequest bookingRequest = actions.NewGetRestRequest(Properties.herokuBaseUrl + Properties.herokuBookings + bookingId);

            bookingRequest.AddHeader("Authorization", "Bearer " + jwt);
            bookingRequest.AddHeader("Accept", "application/json");

            test.Log(Status.Info, "Executing GET request");
            RestResponse responseBooking = await restClient.ExecuteAsync(bookingRequest);

            try
            {
                test.Log(Status.Pass, "Retrieving booking ID information");
                responseBooking.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            catch (Exception)
            {
                test.Log(Status.Fail, "Test Fail");
                throw;
            }
        }

        [Test]
        public async Task PostBookingSuccesfully()
        {
            test.Log(Status.Info, "POST new booking");

            Bookingdates dates = new Bookingdates
            {
                Checkin = DateTime.Now.ToString("MM/dd/yyyy"),
                Checkout = DateTime.Now.ToString("MM/dd/yyyy")
            };

            NewBookingModel booking = new NewBookingModel
            {
                Firstname = "Johhny",
                Lastname = "Dear",
                Totalprice = 12324,
                Depositpaid = true,
                Bookingdates = dates,
                Additionalneeds = "Breakfast"
            };

            RestRequest restRequest = new(Properties.herokuBaseUrl + Properties.herokuBookings, Method.Post);

            test.Log(Status.Info, "Serializing Json body");
            string jSonObject = JsonConvert.SerializeObject(booking);

            test.Log(Status.Info, "Adding necessary headers");
            restRequest.AddStringBody(jSonObject, DataFormat.Json);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Content-Type", "application/json");

            test.Log(Status.Info, "Executing POST request");
            RestResponse responseBooking = await restClient.ExecutePostAsync(restRequest);

            test.Log(Status.Info, "Deserializing response");
            BookingModel responseString = JsonConvert.DeserializeObject<BookingModel>(responseBooking.Content.ToString());

            try
            {
                test.Log(Status.Pass, "Firstname is identical to the model");
                responseString.NewBooking.Firstname.Should().Be("Johhny");
            }
            catch (Exception)
            {
                test.Log(Status.Fail, "Test Fail");
                throw;
            }
        }

        [Test]
        public async Task UpdateBookingSuccessfully()
        {
            test.Log(Status.Info, "Update (PUT) booking by dynamically obtained ID");

            Bookingdates dates = new Bookingdates
            {
                Checkin = DateTime.Now.ToString("MM/dd/yyyy"),
                Checkout = DateTime.Now.ToString("MM/dd/yyyy")
            };

            NewBookingModel booking = new NewBookingModel
            {
                Firstname = "Adamz",
                Lastname = "Dear",
                Totalprice = 12324,
                Depositpaid = true,
                Bookingdates = dates,
                Additionalneeds = "Breakfast"
            };

            jwt = await actions.ObtainJWToken(Properties.authPath);

            newBookingId = await exampleActions.GetBookingId(Properties.herokuBaseUrl + Properties.herokuBookings);

            RestRequest restRequest = new(Properties.herokuBaseUrl + Properties.herokuBookings + newBookingId, Method.Put);

            string jSonObject = JsonConvert.SerializeObject(booking);

            test.Log(Status.Info, "Adding serialized body as JSon");
            restRequest.AddStringBody(jSonObject, DataFormat.Json);

            test.Log(Status.Info, "Adding necessary headers");
            restRequest.AddHeader("Cookie", "token=" + jwt);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Content-Type", "application/json");

            test.Log(Status.Info, "Executing PUT request");
            RestResponse responseBooking = await restClient.ExecutePutAsync(restRequest);

            test.Log(Status.Info, "Deserializing response");
            NewBookingModel responseString = JsonConvert.DeserializeObject<NewBookingModel>(responseBooking.Content.ToString());

            try
            {
                test.Log(Status.Pass, "Firstname is identical to the model");
                responseString.Firstname.Should().Be("Adamz");
            }
            catch (Exception)
            {
                test.Log(Status.Fail, "Test Fail");
                throw;
            }
        }

        [Test]
        public async Task PartialUpdateBookingSuccessfully()
        {
            test.Log(Status.Info, "Partial update (PATCH) booking by dynamically obtained ID");

            jwt = await actions.ObtainJWToken(Properties.authPath);

            newBookingId = await exampleActions.GetBookingId(Properties.herokuBaseUrl + Properties.herokuBookings);

            RestRequest restRequest = new(Properties.herokuBaseUrl + Properties.herokuBookings + newBookingId, Method.Patch);

            test.Log(Status.Info, "Adding serialized body as JSon");
            restRequest.AddStringBody(Properties.firstname, DataFormat.Json);

            test.Log(Status.Info, "Adding necessary headers");
            restRequest.AddHeader("Cookie", "token=" + jwt);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.AddHeader("Content-Type", "application/json");

            test.Log(Status.Info, "Executing PATCH request");
            RestResponse responseBooking = await restClient.PatchAsync(restRequest);

            test.Log(Status.Info, "Deserializing response");
            NewBookingModel responseString = JsonConvert.DeserializeObject<NewBookingModel>(responseBooking.Content.ToString());

            try
            {
                test.Log(Status.Pass, "Firstname is identical to the model");
                responseString.Firstname.Should().Be("Patch");
            }
            catch (Exception)
            {
                test.Log(Status.Fail, "Test Fail");
                throw;
            }
        }

        [Test]
        public async Task DeleteBookingSuccessfully()
        {
            test.Log(Status.Info, "Delete booking by dynamically obtained ID");

            jwt = await actions.ObtainJWToken(Properties.authPath);

            newBookingId = await exampleActions.GetBookingId(Properties.herokuBaseUrl + Properties.herokuBookings);

            RestRequest restRequest = new(Properties.herokuBaseUrl + Properties.herokuBookings + newBookingId, Method.Delete);

            test.Log(Status.Info, "Adding necessary headers");
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Cookie", "token=" + jwt);

            test.Log(Status.Info, "Executing DELETE request");
            RestResponse responseBooking = await restClient.ExecuteAsync(restRequest);

            try
            {
                responseBooking.StatusCode.Should().Be(HttpStatusCode.Created);
                test.Log(Status.Pass, "Expected status code is returned");
            }
            catch (Exception)
            {
                test.Log(Status.Fail, "Test Fail");
                throw;
            }
        }
    }
}
