using System;


namespace Backend.Framework.Utilities
{
    public class Properties
    {
        //REQRES TEST
        public const string baseUrl = "https://reqres.in/api/";
        public const string authPath = "https://restful-booker.herokuapp.com/auth";
        public const string credentials = @"{""username"" : ""admin"", ""password"" : ""password123""}";
        public const string users = "users?page=2";

        //HEROKU APP
        public const string herokuBaseUrl = "https://restful-booker.herokuapp.com/";
        public const string herokuBookings = "booking/";
        public const string firstname = "{\"firstname\" : \"Patch\"}";

        //REPORTER
        public static string projectDirectory = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
        public static string reporterFileLocation = projectDirectory + "Reports" + "\\" + DateTime.Now.ToString("_MMddyyyy_hhmmtt") + "\\" + ".html";
    }
}
