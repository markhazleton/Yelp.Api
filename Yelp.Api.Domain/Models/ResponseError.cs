namespace Yelp.Api.Domain.Models;

public sealed class ResponseError
{
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;
}