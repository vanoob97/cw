using auth5.Models;
using auth5.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Diagnostics;
using System.Security.Claims;

namespace auth5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
        public IActionResult Index()
        {
            List<Item> items = new List<Item>();
            foreach (ItemCollection c in _context.Collections) {
                items.AddRange(c.Items);
            }
            List<ItemCollection> collections = _context.Collections.OrderByDescending(x => x.Items.Count).ToList();
            HomePage home = new HomePage { Items = items.OrderByDescending(x => x.Id).Take(3).ToList<Item>(), Collections = collections.Take(3).ToList() };
            return View(home);
        }
        [Authorize(Roles = "admin")]
        public IActionResult Usermgmt()
        {
            return View(_context.Users);
        }
        public IActionResult SwitchTheme(int id)
        {
            CookieOptions cookies = new CookieOptions();
            cookies.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("theme", id == 0 ? "light" : "dark", cookies);
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}