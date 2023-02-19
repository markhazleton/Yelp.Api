
namespace Yelp.Api.Domain.Test;

[TestClass]
public class ClientTests : IDisposable
{

    public ClientTests(IConfiguration configuration,ILogger logger, IHttpClientFactory factory)
    {
        _configuration = configuration;
        _client = new Client(configuration["YELPAPIKEY"],factory,logger);
    }
    private readonly IConfiguration _configuration;



    private readonly Client _client;


    [TestMethod]
    public void TestSearchCityState()
    {
        var State = "Texas";
        var City = "Dallas";
        var response = _client.SearchCityStateCategory("tacos", City, State);
        Assert.AreNotSame(null, response);
    }
    [TestMethod]
    public void TestSearchCityStateAsync()
    {
        var State = "Texas";
        var City = "Dallas";
        var response = _client.SearchCityStateCategoryAsync("tacos", City, State, 5, true);
        Assert.AreNotSame(null, response);
    }

    [TestMethod]
    public void TestSearch()
    {
        var response = _client.SearchBusinessesAllAsync("cupcakes", 37.786882, -122.399972).Result;

        Assert.AreNotSame(null, response);
        Assert.AreSame(null,
                       response?.Error,
                       $"Response error returned {response?.Error?.Code} - {response?.Error?.Description}");
    }

    [TestMethod]
    public void TestSearchDelivery()
    {
        var response = _client.SearchBusinessesWithDeliveryAsync("mex", 37.786882, -122.399972).Result;

        Assert.AreNotSame(null, response);
        Assert.AreSame(null,
                       response?.Error,
                       $"Response error returned {response?.Error?.Code} - {response?.Error?.Description}");
    }

    [TestMethod]
    public void TestAutocomplete()
    {
        var response = _client.AutocompleteAsync("hot dogs", 37.786882, -122.399972).Result;

        Assert.IsTrue(response.Categories.Length > 0);
        Assert.AreNotSame(null, response);
        Assert.AreSame(null,
                       response?.Error,
                       $"Response error returned {response?.Error?.Code} - {response?.Error?.Description}");
    }

    [TestMethod]
    public void TestGetBusiness()
    {
        var response = _client.GetBusinessAsync("north-india-restaurant-dallas").Result;

        Assert.AreNotSame(null, response);
        Assert.AreSame(null,
                       response?.Error,
                       $"Response error returned {response?.Error?.Code} - {response?.Error?.Description}");
    }

    [TestMethod]
    public void TestGetReviews()
    {
        var response = _client.GetReviewsAsync("north-india-restaurant-dallas").Result;

        Assert.AreNotSame(null, response);
        Assert.AreSame(null,
                       response?.Error,
                       $"Response error returned {response?.Error?.Code} - {response?.Error?.Description}");
    }


    [TestMethod]
    public void TestGetModelChanges()
    {
        var m = new SearchRequest
        {
            Term = "Hello world",
            Price = "$"
        };
        var dic = m.GetChangedProperties();

        Assert.AreEqual(dic.Count, 2);
        Assert.IsTrue(dic.ContainsKey("term"));
        Assert.IsTrue(dic.ContainsKey("price"));
    }
    public void Dispose()
    {
        _client.Dispose();
    }
}
