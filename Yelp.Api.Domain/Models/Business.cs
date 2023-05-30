namespace Yelp.Api.Domain.Models;

public class BusinessResponse : ResponseBase
{
    public const int CoordinateDecimalPlaces = 6;
    public const int DistanceDecimalPlaces = 2;

    private double _DistanceAway = double.NaN;

    /// <summary>
    /// Business Response
    /// </summary>
    public BusinessResponse()
    {
        Reviews = new List<Review>();
    }

    private double ToRadian(double val)
    {
        return (Math.PI / 180) * val;
    }

    /// <summary>
    /// Gets the distance away from a specified location object.
    /// </summary>
    /// <param name="loc">Location object to calculate distance away with.</param>
    /// <returns>Distance amount.</returns>
    public double GetDistanceTo(Coordinates? loc)
    {
        if (loc != null)
        {
            var r = IsMetric ? 6371 : 3960;
            var dLat = ToRadian(this.Coordinates?.Latitude ?? 0 - loc.Latitude);
            var dLon = ToRadian(this.Coordinates?.Longitude ?? 0 - loc.Longitude);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(ToRadian(loc.Latitude)) * Math.Cos(ToRadian(this.Coordinates?.Latitude ?? 0)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            var d = r * c;
            return Math.Round(d, DistanceDecimalPlaces);
        }
        else
            return double.NaN;
    }

    /// <summary>
    /// Sets the distance away property of this object calculated from the specified location object.
    /// </summary>
    /// <param name="loc">Location object to calculate distance away with.</param>
    public void SetDistanceAway(Coordinates? loc)
    {
        if (loc == null) return;
        _DistanceAway = this.GetDistanceTo(loc);
    }

    public void SetDistanceAway(double latitude, double longitude)
    {
        _DistanceAway = this.GetDistanceTo(new Coordinates { Latitude = latitude, Longitude = longitude });
    }

    [JsonProperty("categories")]
    public Category[] Categories { get; set; }

    [JsonProperty("coordinates")]
    public Coordinates Coordinates { get; set; }

    [JsonProperty("display_phone")]
    public string DisplayPhone { get; set; }

    [JsonProperty("distance")]
    public float Distance { get; set; }
    /// <summary>
    /// Gets the distance away this object is. Use SetDistanceAway to update this property.
    /// </summary>
    [JsonIgnore()]
    public double DistanceAway
    {
        get { return _DistanceAway; }
    }

    [JsonProperty("hours")]
    public Hour[] Hours { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("image_url")]
    public string ImageUrl { get; set; }

    [JsonProperty("is_claimed")]
    public bool IsClaimed { get; set; }

    [JsonProperty("is_closed")]
    public bool IsClosed { get; set; }

    /// <summary>
    /// Indicates whether or not the system is metric or imperial.
    /// </summary>
    public static bool IsMetric { get; private set; }

    [JsonProperty("location")]
    public Location Location { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("phone")]
    public string Phone { get; set; }
    [JsonProperty("photos")]
    public string[] Photos { get; set; }

    [JsonProperty("price")]
    public string Price { get; set; }

    [JsonProperty("rating")]
    public float Rating { get; set; }

    [JsonProperty("review_count")]
    public int ReviewCount { get; set; }
    public List<Review> Reviews { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

}