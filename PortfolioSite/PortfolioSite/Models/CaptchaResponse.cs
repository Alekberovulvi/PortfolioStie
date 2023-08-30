using Newtonsoft.Json;

namespace PortfolioSite.Models
{
    public class CaptchaResponse
    {
        public int Id { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}
