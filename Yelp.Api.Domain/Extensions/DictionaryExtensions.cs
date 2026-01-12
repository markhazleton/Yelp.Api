
namespace Yelp.Api.Domain.Extensions;

public static class DictionaryExtensions
{
    public static IEnumerable<KeyValuePair<T, S>> ToKeyValuePairList<T, S>(this Dictionary<T, S> dictionary) where T : notnull
    {
        var list = new List<KeyValuePair<T, S>>();
        foreach (var pair in dictionary)
            list.Add(pair);
        return list;
    }

    public static string ToQueryString(this Dictionary<string, object> dictionary)
    {
        string querystring = string.Empty;
        List<string> parameters = new List<string>();

        if (dictionary == null)
            return querystring;

        foreach (var pair in dictionary.Where(w => w.Value != null))
            parameters.Add(string.Join("=", pair.Key, Uri.EscapeDataString(pair.Value?.ToString() ?? string.Empty)));

        if (parameters.Count > 0)
            querystring = $"?{(string.Join("&", parameters))}";

        return querystring;
    }

    public static string InitCapitalConvert(this string input_string)
    {
        input_string = $" {input_string.ToLower()}";

        string[] AlphabatList = new string[] {"a","b","c","d","e","f","g",
    "h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};

        foreach (string alpha in AlphabatList)
        {
            input_string = input_string.Replace($" {alpha}", $" {alpha.ToUpper()}");
        }
        return input_string;
    }


}