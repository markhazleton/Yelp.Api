
namespace Yelp.Api.Domain.Models;

public class AutocompleteResponse : ResponseBase
{
    [JsonProperty("businesses")]
    public BusinessResponse[] Businesses { get; set; }

    [JsonProperty("categories")]
    public Category[] Categories { get; set; }

    [JsonProperty("terms")]
    public Term[] Terms { get; set; }
}