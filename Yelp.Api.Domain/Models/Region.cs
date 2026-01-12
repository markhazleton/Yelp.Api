namespace Yelp.Api.Domain.Models;

public class Region
{
    [JsonProperty("center")]
    public Coordinates Center { get; set; } = new();
}