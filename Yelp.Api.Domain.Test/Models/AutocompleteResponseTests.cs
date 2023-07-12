namespace Yelp.Api.Domain.Test.Models;

[TestClass]
public class AutocompleteResponseTests
{
    [TestMethod]
    public void AutocompleteResponse_Expected()
    {
        // Arrange
        var autocompleteResponse = new AutocompleteResponse();

        // Act


        // Assert
        Assert.IsNotNull(autocompleteResponse);
    }
}
