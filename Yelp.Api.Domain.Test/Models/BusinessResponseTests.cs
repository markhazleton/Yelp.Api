using System;

namespace Yelp.Api.Domain.Test.Models
{
    [TestClass]
    public class BusinessResponseTests
    {
        [TestMethod]
        public void SetDistanceAway_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var businessResponse = new BusinessResponse();
            Coordinates? loc = null;

            // Act
            businessResponse.SetDistanceAway(loc);

            // Assert
            Assert.IsNotNull(businessResponse);
        }

        [TestMethod]
        public void SetDistanceAway_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var businessResponse = new BusinessResponse();
            double latitude = 0;
            double longitude = 0;

            // Act
            businessResponse.SetDistanceAway(
                latitude,
                longitude);

            // Assert
            Assert.IsNotNull(businessResponse);

        }

        [TestMethod]
        public void GetDistanceTo_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var businessResponse = new BusinessResponse();
            Coordinates? loc = null;

            // Act
            var result = businessResponse.GetDistanceTo(loc);

            // Assert
            Assert.IsNotNull(businessResponse);

        }
    }
}
