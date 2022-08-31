
namespace Yelp.Api.Web.Controllers;

/// <summary>
/// BaseController
/// </summary>
[ApiExplorerSettings(IgnoreApi = true)]
public abstract class BaseController : Controller
{
    /// <summary>
    /// 
    /// </summary>
    protected readonly Client _client;

    /// <summary>
    ///
    /// </summary>
    public readonly CancellationTokenSource cts;
    /// <summary>
    /// 
    /// </summary>
    protected readonly IConfiguration _configuration;

    /// <summary>
    /// BaseController
    /// </summary>
    protected BaseController(IConfiguration configuration)
    {
        cts = new CancellationTokenSource();
        _configuration = configuration;
        _client = new Client(configuration["YELPAPIKEY"]);
    }
    protected ApplicationStatus GetApplicationStatus()
    {
        return new ApplicationStatus(Assembly.GetExecutingAssembly());
    }
}
