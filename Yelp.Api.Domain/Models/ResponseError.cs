
namespace Yelp.Api.Domain.Models;

public sealed class ResponseError
{
    [JsonProperty("code")]
    public string Code { get; set; }
    [JsonProperty("description")]
    public string Description { get; set; }
}