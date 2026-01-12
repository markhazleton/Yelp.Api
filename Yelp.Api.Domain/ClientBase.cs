using System.Diagnostics;

namespace Yelp.Api.Domain;

/// <summary>
/// Base class for any SDK client API implementation containing reusable logic for common call types, error handling, request retry attempts.
/// </summary>
public abstract class ClientBase : IDisposable
{
    public const int E_WINHTTP_CANNOT_CONNECT = unchecked((int)0x80072efd);
    public const int E_WINHTTP_CONNECTION_ERROR = unchecked((int)0x80072efe);
    public const int E_WINHTTP_NAME_NOT_RESOLVED = unchecked((int)0x80072ee7);
    public const int E_WINHTTP_TIMEOUT = unchecked((int)0x80072ee2);
    private ILogger? _logger;


    public ClientBase(string baseURL, IHttpClientFactory factory, ILogger? logger = null)
    {
        this.BaseUri = new Uri(baseURL);
        this.Client = factory.CreateClient("YelpAPI");
        _logger = logger;
    }

    private void Log(string message)
    {
        if (_logger != null)
            _logger.Log(message);
        else
            Debug.WriteLine(message);
    }

    /// <summary>
    /// Logs HttpRequest information to the application logger.
    /// </summary>
    /// <param name="request">Request to log.</param>
    private void Log(HttpRequestMessage request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        try
        {
            var message = string.Format(
                $"{Environment.NewLine}---------------------------------{Environment.NewLine}WEB REQUEST to {{0}}{Environment.NewLine}-Method: {{1}}{Environment.NewLine}-Headers: {{2}}{Environment.NewLine}-Contents: {Environment.NewLine}{{3}}{Environment.NewLine}---------------------------------",
                request.RequestUri?.OriginalString ?? "unknown",
                request.Method.Method,
                request.Headers?.ToString() ?? string.Empty,
                request.Content?.ReadAsStringAsync().Result ?? string.Empty
            );
            this.Log(message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error during Log(HttpRequestMessage request): {ex}");
        }
    }

    /// <summary>
    /// Logs the HttpResponse object to the application logger.
    /// </summary>
    /// <param name="response">Response to log.</param>
    private void Log(HttpResponseMessage response)
    {
        if (response == null)
            throw new ArgumentNullException(nameof(response));

        if (response.RequestMessage != null)
            this.Log(response.RequestMessage);

        try
        {
            var message = string.Format(
                $"{Environment.NewLine}---------------------------------{Environment.NewLine}WEB RESPONSE to {{0}}{Environment.NewLine}-HttpStatus: {{1}}{Environment.NewLine}-Reason Phrase: {{2}}{Environment.NewLine}-ContentLength: {{3:0.00 KB}}{Environment.NewLine}-Contents: {Environment.NewLine}{{4}}{Environment.NewLine}---------------------------------",
                response.RequestMessage?.RequestUri?.OriginalString ?? "unknown",
                $"{(int)response.StatusCode} {response.StatusCode.ToString()}",
                response.ReasonPhrase ?? "unknown",
                Convert.ToDecimal(Convert.ToDouble(response.Content.Headers.ContentLength) / 1024),
                response.Content?.ReadAsStringAsync().Result ?? string.Empty
                );
            this.Log(message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error during Log(HttpResponseMessage request): {ex}");
        }
    }




    /// <summary>
    /// Gets data from the specified URL.
    /// </summary>
    /// <typeparam name="T">Type for the strongly typed class representing data returned from the URL.</typeparam>
    /// <param name="url">URL to retrieve data from.</param>should be deserialized.</param>
    /// <param name="retryCount">Number of retry attempts if a call fails. Default is zero.</param>
    /// <param name="serializerType">Specifies how the data should be deserialized.</param>
    /// <returns>Instance of the type specified representing the data returned from the URL.</returns>
    /// <summary>
    protected async Task<T?> GetAsync<T>(string url, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentNullException(nameof(url));

        var response = await this.Client.GetAsync(new Uri(this.BaseUri, url), ct).ConfigureAwait(false);
        this.Log(response);
        var data = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        var jsonModel = JsonConvert.DeserializeObject<T>(data, settings);

        return jsonModel;
    }

    /// <summary>
    /// Posts data to the specified URL.
    /// </summary>
    /// <typeparam name="T">Type for the strongly typed class representing data returned from the URL.</typeparam>
    /// <param name="url">URL to retrieve data from.</param>
    /// <param name="contents">Any content that should be passed into the post.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <param name="serializerType">Specifies how the data should be deserialized.</param>
    /// <returns>Instance of the type specified representing the data returned from the URL.</returns>
    protected async Task<T?> PostAsync<T>(string url, CancellationToken ct, HttpContent? contents = null)
    {
        string data = await this.PostAsync(url, ct, contents).ConfigureAwait(false);

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        var jsonModel = JsonConvert.DeserializeObject<T>(data, settings);

        return jsonModel;
    }

    /// <summary>
    /// Posts data to the specified URL.
    /// </summary>
    /// <param name="url">URL to retrieve data from.</param>
    /// <param name="contents">Any content that should be passed into the post.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <param name="serializerType">Specifies how the data should be deserialized.</param>
    /// <returns>Response contents as string else null if nothing.</returns>
    protected async Task<string> PostAsync(string url, CancellationToken ct, HttpContent? contents = null)
    {
        HttpResponseMessage response = await this.PostAsync(url, contents, ct).ConfigureAwait(false);
        var data = await response.Content.ReadAsStringAsync();
        return data ?? string.Empty;
    }

    /// <summary>
    /// Posts data to the specified URL.
    /// </summary>
    /// <param name="url">URL to retrieve data from.</param>
    /// <param name="contents">Any content that should be passed into the post.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <param name="serializerType">Specifies how the data should be deserialized.</param>
    /// <returns>Response contents as string else null if nothing.</returns>
    protected async Task<HttpResponseMessage> PostAsync(string url, HttpContent? contents, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentNullException(nameof(url));

        var response = await this.Client.PostAsync(new Uri(this.BaseUri, url), contents, ct).ConfigureAwait(false);
        this.Log(response);
        return response;
    }

    protected Uri BaseUri { get; private set; }

    protected HttpClient Client { get; private set; }

    public void Dispose()
    {
        this.Client.Dispose();
    }


}