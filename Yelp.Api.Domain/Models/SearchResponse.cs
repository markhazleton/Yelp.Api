
namespace Yelp.Api.Domain.Models;

public class SearchResponse : ResponseBase
{

    [JsonProperty("businesses")]
    public List<BusinessResponse> Businesses { get; set; } = new List<BusinessResponse>();
    [JsonProperty("region")]
    public Region Region { get; set; } = new Region();

    [JsonProperty("total")]
    public int Total { get; set; }
    /// <summary>
    /// Terms Used for Search
    /// </summary>
    public string? Term { get; set; }


}