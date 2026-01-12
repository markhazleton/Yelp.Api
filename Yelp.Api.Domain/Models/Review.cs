namespace Yelp.Api.Domain.Models;

public class Review
{

    [JsonProperty("rating")]
    public int Rating { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; } = string.Empty;

    [JsonProperty("time_created")]
    public string TimeCreated { get; set; } = string.Empty;
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    [JsonProperty("user")]
    public User User { get; set; } = new();
}