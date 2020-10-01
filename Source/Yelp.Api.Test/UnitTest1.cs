using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Yelp.Api.Models;

namespace Yelp.Api.Test
{
    [TestClass]
    public class UnitTest1 : IDisposable
    {
        #region Constructors

        public UnitTest1()
        {
            _client = new Client(API_KEY);
        }

        #endregion

        #region Variables

        private const string API_KEY = "OMJiy42P85UdkilYO-V8dC98cA9Y8HQlsr5QVijwltwqeSdNGPmgFfL7_921BDgzl_8z-sfe8i5zroWt_vogzfq2th4XlufZ2xqASsOkQ0sOBpoaRlemA6UM9EyjWnYx";

        private readonly Client _client;

        #endregion

        #region Methods
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
            var response = _client.SearchCityStateCategoryAsync("tacos", City, State,5,true);
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
            var response = _client.GetBusinessAsync("north-india-restaurant-san-francisco").Result;

            Assert.AreNotSame(null, response);
            Assert.AreSame(null,
                           response?.Error,
                           $"Response error returned {response?.Error?.Code} - {response?.Error?.Description}");
        }

        [TestMethod]
        public void TestGetReviews()
        {
            var response = _client.GetReviewsAsync("north-india-restaurant-san-francisco").Result;

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
        #endregion
    }
}