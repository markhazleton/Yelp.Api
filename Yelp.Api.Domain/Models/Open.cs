
namespace Yelp.Api.Domain.Models;

public class Open
{

    private string FormatTime(string time)
    {
        try
        {
            var dt = DateTime.Today.AddHours(double.Parse(time.Substring(0, 2))).AddMinutes(double.Parse(time.Substring(2)));
            return dt.ToString("h:mmtt").ToLowerInvariant();
        }
        catch
        {
            return time;
        }
    }

    public override string ToString()
    {
        return $"{(DayOfWeek)Day}: {FormatTime(Start)} - {FormatTime(End)}";
    }

    [JsonProperty("day")]
    public int Day { get; set; }
    [JsonProperty("end")]
    public string End { get; set; }

    [JsonProperty("is_overnight")]
    public bool IsOvernight { get; set; }

    [JsonProperty("start")]
    public string Start { get; set; }
}