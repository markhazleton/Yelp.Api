# Yelp.API
C# Class Library for Yelp (v3) Fusion API works with .NET projects. 
Yelp's (v3) Fusion API allows you to get the best local business information and user reviews of over million businesses in 32 countries.

## How to use the Yelp API in your .NET based app

Integrating this API is very easy.

1. **[Register](https://www.yelp.com/developers/v3/manage_app)** yourself with Yelp's developer program

2. Add the API_KEY to the secrets file for both Test and Web projects

```c#
    var client = new Yelp.Api.Client("API_KEY");
    var results = await client.SearchBusinessesAllAsync("cupcakes", 37.786882, -122.399972);
```
or if you want to perform a more advanced search, use the `SearchParameters` object.

```c#
    var request = new Yelp.Api.Models.SearchRequest();
    request.Latitude = 37.786882;
    request.Longitude = -122.399972;
    request.Term = "cupcakes";
    request.MaxResults = 40;
    request.OpenNow = true;

    var client = new Yelp.Api.Client("API_KEY");
    var results = await client.SearchBusinessesAllAsync(request);
```


