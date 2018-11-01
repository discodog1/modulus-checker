using Newtonsoft.Json;

namespace ModulusChecker.API
{
    public class BankAccountModel
    {
        [JsonProperty("sortCode")]
        public string SortCode { get; set; }
        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }
    }
}
