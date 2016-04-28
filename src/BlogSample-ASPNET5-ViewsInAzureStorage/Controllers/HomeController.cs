using System;
using BlogSample_ASPNET5_ViewsInAzureStorage.Models;
using Microsoft.AspNet.Mvc;

namespace BlogSample_ASPNET5_ViewsInAzureStorage.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            var model = new WelcomeModel
            {
                DisplayName = "Simon Bull",
                LastSeen = DateTime.Today.AddDays(-1).AddHours(10),
                NumberOfMessages = new Random().Next(5, 10)
            };
            return View(model);
        }
    }
}
