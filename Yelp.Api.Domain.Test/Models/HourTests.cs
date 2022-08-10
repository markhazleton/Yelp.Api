
namespace Yelp.Api.Domain.Test.Models;

[TestClass]
public class HourTests
{
    [TestMethod]
    public void OpenTest_Valid()
    {
        // Arrange
        var hour = new Hour()
        {
            HoursType = "test",
            IsOpenNow = false,
            Open = new Open[] { GetOpen(1), GetOpen(2) }
        };

        // Act

        // Assert
        Assert.IsNotNull(hour);
    }
    private Open GetOpen(int dayOfWeek)
    {
        return new Open()
        {
            Start = "0800",
            Day = dayOfWeek,
            End = "2100",
            IsOvernight = false
        };
    }
}
