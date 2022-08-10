
namespace Yelp.Api.Domain.Models;

public class Coordinates : ICoordinates
{
    public Coordinates()
    {
        Latitude = double.NaN;
        Longitude = double.NaN;
    }

    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("longitude")]
    public double Longitude { get; set; }
}