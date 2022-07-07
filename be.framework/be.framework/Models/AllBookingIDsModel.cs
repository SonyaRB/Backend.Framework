using Newtonsoft.Json;

namespace Backend.Framework.Models
{
    public class AllBookingIDsModel
    {
        [JsonProperty("bookingid")]
        public int Id { get; set; }
    }
}
