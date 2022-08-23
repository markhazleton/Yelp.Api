
namespace Yelp.Api.Domain.Models;

public class SearchResponse : ResponseBase
{

    [JsonProperty("businesses")]
    public List<BusinessResponse> Businesses { get; set; }
    [JsonProperty("region")]
    public Region Region { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }
    /// <summary>
    /// Terms Used for Search
    /// </summary>
    public string? Term { get; set; }
}