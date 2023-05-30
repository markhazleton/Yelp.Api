namespace Yelp.Api.Domain.Models;

public abstract class ResponseBase : ModelBase
{
    [JsonProperty("error")]
    public ResponseError Error { get; set; }
}