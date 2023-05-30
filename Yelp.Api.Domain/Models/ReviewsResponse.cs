namespace Yelp.Api.Domain.Models;

public class ReviewsResponse : ResponseBase
{
    [JsonProperty("reviews")]
    public Review[] Reviews { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }
}