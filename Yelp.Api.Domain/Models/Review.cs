namespace Yelp.Api.Domain.Models;

public class Review
{

    [JsonProperty("rating")]
    public int Rating { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("time_created")]
    public string TimeCreated { get; set; }
    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("user")]
    public User User { get; set; }
}