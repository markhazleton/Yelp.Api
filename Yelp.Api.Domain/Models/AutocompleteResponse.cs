using System.Text.Json;

namespace Yelp.Api.Domain.Models;

public class AutocompleteResponse : ResponseBase
{
    [JsonProperty("businesses")]
    public BusinessResponse[] Businesses { get; set; } = Array.Empty<BusinessResponse>();

    [JsonProperty("categories")]
    public Category[] Categories { get; set; } = Array.Empty<Category>();

    [JsonProperty("terms")]
    public Term[] Terms { get; set; } = Array.Empty<Term>();
}