namespace Yelp.Api.Domain.Models
{
    public class Term
    {
        [JsonProperty("text")]
        public string Text { get; set; } = string.Empty;
    }
}