using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Xml.Serialization;

namespace Yelp.Api.Domain;

/// <summary>
/// Client class to access Yelp Fusion v3 API.
/// </summary>
public sealed class Client : ClientBase
{
    private const string API_VERSION = "/v3";
    private const string BASE_ADDRESS = "https://api.yelp.com";
    private readonly ObjectCache cache = MemoryCache.Default;
    /// <summary>
    /// Constructor for the Client class.
    /// </summary>
    /// <param name="apiKey">App secret from yelp's developer registration page.</param>
    /// <param name="logger">Optional class instance which applies the ILogger interface to support custom logging within the client.</param>
    public Client(string apiKey, IHttpClientFactory factory, ILogger logger=null) : base(BASE_ADDRESS, factory, logger)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentNullException(nameof(apiKey));

        ApiKey = apiKey;
    }

    private void ApplyAuthenticationHeaders()
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
    }

    /// <summary>
    /// Validates latitude and longitude values. Throws an ArgumentOutOfRangeException if not in the valid range of values.
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    private void ValidateCoordinates(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentOutOfRangeException(nameof(latitude));
        else if (longitude < -180 || latitude > 180)
            throw new ArgumentOutOfRangeException(nameof(longitude));
    }

    private string ApiKey
    {
        get;
        set;
    }

    /// <summary>
    /// Searches businesses matching the specified search text used in a client search autocomplete box.
    /// </summary>
    /// <param name="term">Text to search businesses with.</param>
    /// <param name="latitude">User's current latitude.</param>
    /// <param name="longitude">User's current longitude.</param>
    /// <param name="locale">Language/locale value from https://www.yelp.com/developers/documentation/v3/supported_locales </param>
    /// <param name="ct">Cancellation token instance. Use CancellationToken.None if not needed.</param>
    /// <returns>AutocompleteResponse with businesses/categories/terms matching the specified parameters.</returns>
    public async Task<AutocompleteResponse> AutocompleteAsync(string text,
                                                              double latitude,
                                                              double longitude,
                                                              string locale = null,
                                                              CancellationToken ct = default)
    {
        ValidateCoordinates(latitude, longitude);
        ApplyAuthenticationHeaders();

        var dic = new Dictionary<string, object>
        {
            { "text", text },
            { "latitude", latitude },
            { "longitude", longitude }
        };
        if (!string.IsNullOrEmpty(locale))
            dic.Add("locale", locale);
        string querystring = dic.ToQueryString();

        var response = await this.GetAsync<AutocompleteResponse>($"{API_VERSION}/autocomplete{querystring}", ct)
        .ConfigureAwait(false);

        // Set distances baased on lat/lon
        if (response?.Businesses != null && !double.IsNaN(latitude) && !double.IsNaN(longitude))
            foreach (var business in response.Businesses)
                business.SetDistanceAway(latitude, longitude);

        return response;
    }

    /// <summary>
    /// Gets details of a business based on the provided ID value.
    /// </summary>
    /// <param name="businessID">ID value of the Yelp business.</param>
    /// <param name="ct">Cancellation token instance. Use CancellationToken.None if not needed.</param>
    /// <returns>BusinessResponse instance with details of the specified business if found.</returns>
    public async Task<BusinessResponse> GetBusinessAsync(string businessID,
                                                         CancellationToken ct = default)
    {
        ApplyAuthenticationHeaders();
        return await GetAsync<BusinessResponse>($"{API_VERSION}/businesses/{Uri.EscapeDataString(businessID)}", ct)
        .ConfigureAwait(false);
    }

    /// <summary>
    /// Gets user reviews of a business based on the provided ID value.
    /// </summary>
    /// <param name="businessID">ID value of the Yelp business.</param>
    /// <param name="locale">Language/locale value from https://www.yelp.com/developers/documentation/v3/supported_locales </param>
    /// <param name="ct">Cancellation token instance. Use CancellationToken.None if not needed.</param>
    /// <returns>ReviewsResponse instance with reviews of the specified business if found.</returns>
    public async Task<ReviewsResponse> GetReviewsAsync(string businessID,
                                                       string locale = null,
                                                       CancellationToken ct = default)
    {
        ApplyAuthenticationHeaders();
        var dic = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(locale))
            dic.Add("locale", locale);
        string querystring = dic.ToQueryString();
        return await this.GetAsync<ReviewsResponse>($"{API_VERSION}/businesses/{Uri.EscapeDataString(businessID)}/reviews{querystring}",
                                                    ct)
        .ConfigureAwait(false);
    }

    /// <summary>
    /// Searches any and all businesses matching the data in the specified search parameter object.
    /// </summary>
    /// <param name="search">Container object for all search parameters.</param>
    /// <param name="ct">Cancellation token instance. Use CancellationToken.None if not needed.</param>
    /// <returns>SearchResponse with businesses matching the specified parameters.</returns>
    public async Task<SearchResponse> SearchBusinessesAllAsync(SearchRequest search,
                                                               CancellationToken ct = default)
    {
        if (search == null)
            throw new ArgumentNullException(nameof(search));

        ValidateCoordinates(search.Latitude, search.Longitude);
        ApplyAuthenticationHeaders();

        var querystring = search.GetChangedProperties().ToQueryString();
        var response = await this.GetAsync<SearchResponse>($"{API_VERSION}/businesses/search{querystring}", ct)
        .ConfigureAwait(false);

        // Set distances based on lat/lon
        if (response?.Businesses != null && !double.IsNaN(search.Latitude) && !double.IsNaN(search.Longitude))
            foreach (var business in response.Businesses)
                business.SetDistanceAway(search.Latitude, search.Longitude);

        return response;
    }

    /// <summary>
    /// Searches any and all businesses matching the specified search text.
    /// </summary>
    /// <param name="term">Text to search businesses with.</param>
    /// <param name="latitude">User's current latitude.</param>
    /// <param name="longitude">User's current longitude.</param>
    /// <param name="ct">Cancellation token instance. Use CancellationToken.None if not needed.</param>
    /// <returns>SearchResponse with businesses matching the specified parameters.</returns>
    public Task<SearchResponse> SearchBusinessesAllAsync(string term,
                                                         double latitude,
                                                         double longitude,
                                                         CancellationToken ct = default)
    {
        SearchRequest search = new SearchRequest();
        if (!string.IsNullOrEmpty(term))
            search.Term = term;
        search.Latitude = latitude;
        search.Longitude = longitude;
        return SearchBusinessesAllAsync(search, ct);
    }

    /// <summary>
    /// Searches businesses that deliver matching the specified search text.
    /// </summary>
    /// <param name="term">Text to search businesses with.</param>
    /// <param name="latitude">User's current latitude.</param>
    /// <param name="longitude">User's current longitude.</param>
    /// <param name="ct">Cancellation token instance. Use CancellationToken.None if not needed.</param>
    /// <returns>SearchResponse with businesses matching the specified parameters.</returns>
    public async Task<SearchResponse> SearchBusinessesWithDeliveryAsync(string term,
                                                                        double latitude,
                                                                        double longitude,
                                                                        CancellationToken ct = default)
    {
        ValidateCoordinates(latitude, longitude);
        ApplyAuthenticationHeaders();

        var dic = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(term))
            dic.Add("term", term);
        dic.Add("latitude", latitude);
        dic.Add("longitude", longitude);
        string querystring = dic.ToQueryString();
        var response = await this.GetAsync<SearchResponse>($"{API_VERSION}/transactions/delivery/search{querystring}",
                                                           ct)
        .ConfigureAwait(false);

        // Set distances baased on lat/lon
        if (response?.Businesses != null && !double.IsNaN(latitude) && !double.IsNaN(longitude))
            foreach (var business in response.Businesses)
                business.SetDistanceAway(latitude, longitude);

        return response;
    }



    /// <summary>
    /// SearchCityStateCategory
    /// </summary>
    /// <param name="term"></param>
    /// <param name="city"></param>
    /// <param name="state"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public string SearchCityStateCategory(string term,
                                          string city,
                                          string state,
                                          int limit = 5)
    {
        CancellationToken ct = default;
        SearchRequest search = new SearchRequest
        {
            MaxResults = limit
        };
        if (!string.IsNullOrEmpty(term))
            search.Term = term;

        if (!string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(state))
        {
            search.Location = $"{city}, {state}";
        }
        SearchResponse result = SearchBusinessesAllAsync(search, ct).Result;
        XmlSerializer writer = new XmlSerializer(typeof(SearchResponse));
        StringWriter sw = new StringWriter();
        writer.Serialize(sw, result);
        return sw.ToString();
    }

    /// <summary>
    /// Searches any and all businesses matching the specified search text.
    /// </summary>
    /// <param name="term">Text to search businesses with.</param>
    /// <param name="city">City.</param>
    /// <param name="state">State.</param>
    /// <param name="ct">Cancellation token instance. Use CancellationToken.None if not needed.</param>
    /// <returns>SearchResponse with businesses matching the specified parameters.</returns>
    public async Task<SearchResponse> SearchCityStateCategoryAsync(string term,
                                                             string city,
                                                             string state,
                                                             int limit = 5,
                                                             bool details = false,
                                                             CancellationToken ct = default)
    {
        term = term.InitCapitalConvert();

        string cacheKey = $"{city}:{state}:{term}";
        SearchResponse? cacheContents = (SearchResponse)cache.Get(cacheKey);
        if (cacheContents != null) return cacheContents;

        CacheItemPolicy policy = new()
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddHours(10.0)
        };
        SearchRequest search = new()
        {
            MaxResults = limit,
            SortBy = "rating"
        };

        if (!string.IsNullOrEmpty(term))
            search.Term = term;

        if (!string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(state))
        {
            search.Location = $"{city}, {state}";
        }
        if (details)
        {
            cacheContents = SearchBusinessesAllAsync(search, ct).Result;
            var bizList = cacheContents.Businesses.Select(s => s.Id).ToArray();
            cacheContents.Businesses.Clear();
            foreach (var biz in bizList)
            {
                cacheContents.Businesses.Add(GetBusinessAsync(biz).Result);
            }

            foreach (var biz in cacheContents.Businesses)
            {
                biz.Reviews.AddRange(GetReviewsAsync(biz.Id).Result.Reviews);
            }
        }
        else
        {
            cacheContents = await SearchBusinessesAllAsync(search, ct);
        }
        cacheContents.City = city;
        cacheContents.State = state;
        cacheContents.Term = term;
        cacheContents.RequestTime = DateTime.Now;
        cache.Set(cacheKey, cacheContents, policy);
        return cacheContents;
    }

}