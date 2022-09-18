
namespace Yelp.Api.Web.Controllers;
/// <summary>
/// Home Controller
/// </summary>
public class HomeController : BaseController
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public HomeController(IConfiguration configuration) : base(configuration)
    {

    }
    /// <summary>
    /// Error Page Display
    /// </summary>
    /// <returns></returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    { return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); }

    /// <summary>
    /// Main Home Page
    /// </summary>
    /// <returns></returns>
    public ActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="city"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public async Task<ActionResult> Category(string id = "bars", string state = "Kansas", string city = "Wichita")
    {
        var yelpReturn = await _client.SearchCityStateCategoryAsync(id, city, state, 10, true);
        return View(yelpReturn);
    }
}

