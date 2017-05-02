using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Threading.Tasks;
using Pingpp.Net;

namespace Pingpp.Models {
    public class Token: Pingpp {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("created")]
        public int? Created { get; set; }

        [JsonProperty("livemode")]
        public bool Livemode { get; set; }

        [JsonProperty("used")]
        public bool Used { get; set; }

        [JsonProperty("time_used")]
        public int? TimeUsed { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("card")]
        public Dictionary<string, object> Card { get; set; }

        [JsonProperty("sms_code")]
        public Dictionary<string, object> SmsCode { get; set; }

        private const string BaseUrl = "/v1/tokens";

        public static async Task<Token> Create(Dictionary<string, object> cardParams) {
            var tok = await Requestor.DoRequest(BaseUrl, "POST", cardParams);
            return Mapper<Token>.MapFromJson(tok);
        }

        public static async Task<Token> Retrieve(string tokId) {
            var url = string.Format("{0}/{1}", BaseUrl, tokId);
            var tok = await Requestor.DoRequest(url, "GET");
            return Mapper<Token>.MapFromJson(tok);
        }

    }
}