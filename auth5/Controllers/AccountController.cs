using auth5.Models;
using auth5.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace auth5.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext _context;
        public AccountController(ApplicationContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> PersonalPage(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            string email = user.Email;
            List<ItemCollection> collections = await _context.Collections.Where(c => c.OwnerEmail == email).ToListAsync<ItemCollection>();
            UserCollections uc =  new UserCollections { Id = id, Email = email, Collections = collections };
            return View("MyPage", uc);
        }
        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> MyPage()
        {
            String email = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            List<ItemCollection> collections = await _context.Collections.Where(c => c.OwnerEmail == email).ToListAsync<ItemCollection>();
            UserCollections uc = new UserCollections { Id = user.Id, Email = email, Collections = collections };
            return View(uc);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    user = new User { Name = model.Name, Email = model.Email, Password = model.Password, CreatedDate = DateTime.Now, LastLogin = DateTime.Now, Status = "Normal" };
                    Role userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                    if (userRole != null)
                        user.Role = userRole;
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                    await Authenticate(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "This email is already taken");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    if (user.Status == "Blocked") {
                        ModelState.AddModelError("", "This account is blocked");
                        return View(model);
                    }
                    await Authenticate(user);
                    user.LastLogin = DateTime.Now;
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Incorrect login and/or password");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public JsonResult Block(string[] id)
        {
            return ProcessUsers(id, (x) => { 
                x.Status = "Blocked";
                _context.Users.Update(x);
            });
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            return ProcessUsers(id, (x) => {
                _context.Users.Remove(x); 
            });
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public JsonResult Unblock(string[] id)
        {
            return ProcessUsers(id, (x) => { 
                x.Status = "Normal";
                _context.Users.Update(x);
            });
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public JsonResult Promote(string[] id)
        {
            return ProcessUsers(id, (x) => {
                x.RoleId = 1;
                _context.Users.Update(x);
            });
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public JsonResult Demote(string[] id)
        {
            return ProcessUsers(id, (x) => {
                x.RoleId = 2;
                _context.Users.Update(x);
            });
        }
        private JsonResult ProcessUsers(string[] id, Action<User> process) {
            var users = _context.Users
                .Where(u => id.Contains(u.Id.ToString()) && u.Status != "Immune");
            foreach (var user in users)
            {
                process(user);
            }
            _context.SaveChanges();
            return Json(true);
        }
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
