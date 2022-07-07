using Newtonsoft.Json;

namespace Backend.Framework.Models
{
    public class BookingModel
    {
        [JsonProperty("bookingid")]
        public int bookingid { get; set; }

        [JsonProperty("booking")]
        public NewBookingModel NewBooking { get; set; }
    }
}
