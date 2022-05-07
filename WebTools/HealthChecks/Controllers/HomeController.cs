using Microsoft.AspNetCore.Mvc;

namespace HealthChecks.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Redirect("/healthchecks-ui");
    }
}