namespace Yelp.Api.Domain.Models;

public class User
{

    [JsonProperty("image_url")]
    public string ImageUrl { get; set; } = string.Empty;
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}