using Newtonsoft.Json;

namespace Backend.Framework.Models
{
    public class TokenModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
