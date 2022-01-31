using auth5.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace auth5.Controllers
{
    public class CollectionsController : Controller
    {
        private ApplicationContext _context;
        public CollectionsController(ApplicationContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> EditCollection(int id)
        {
            ItemCollection collection = await _context.Collections.FirstOrDefaultAsync(u => u.Id == id);
            ViewBag.Collection = collection;
            return View();
        }
        [Authorize(Roles = "admin, user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCollection(EditCollectionModel model)
        {
            ItemCollection collection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == model.Id);
            if (ModelState.IsValid)
            {
                collection.Title = model.Title;
                collection.Description = model.Description;
                collection.NumFields = String.Join("|", model.NumFields.ToArray());
                collection.StrFields = String.Join("|", model.StrFields.ToArray());
                collection.TextFields = String.Join("|", model.TextFields.ToArray());
                collection.BoolFields = String.Join("|", model.BoolFields.ToArray());
                _context.Collections.Update(collection);
                await _context.SaveChangesAsync();
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == collection.OwnerEmail);
                return RedirectToAction("PersonalPage", "Account", new { id = user.Id });
            }
            ViewBag.Collection = collection;
            return View();
        }
        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> AddCollection(int id)
        {
            IEnumerable<string> themes = new List<string>{ 
                "Books",
                "Movies",
                "Games",
                "Alcohol",
                "Animals",
                "Vehicles"
            };
            ViewBag.Themes = new SelectList(themes);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            ViewBag.Email = user.Email;
            return View();
        }
        [Authorize(Roles = "admin, user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCollection(AddCollectionModel model)
        {
            if (ModelState.IsValid)
            {
                ItemCollection collection = new ItemCollection { Title = model.Title, OwnerEmail = model.Email, Description = model.Description, ImgSrc = "",
                    Theme = model.Theme, Items = new List<Item>(), NumFields = String.Join("|", model.NumFields.Where(s => !String.IsNullOrWhiteSpace(s)).ToArray()),
                    StrFields = String.Join("|", model.StrFields.Where(s => !String.IsNullOrWhiteSpace(s)).ToArray()),
                    TextFields = String.Join("|", model.TextFields.Where(s => !String.IsNullOrWhiteSpace(s)).ToArray()),
                    BoolFields = String.Join("|", model.BoolFields.Where(s => !String.IsNullOrWhiteSpace(s)).ToArray())
                };
                _context.Collections.Add(collection);
                await _context.SaveChangesAsync();
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == collection.OwnerEmail);
                return RedirectToAction("PersonalPage", "Account", new { id = user.Id });
            }
            ViewBag.Email = model.Email;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> OpenCollection(int id)
        {
            ItemCollection collection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == id);
            return View(collection);
        }
        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            ItemCollection collection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == id);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == collection.OwnerEmail);
            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();
            return RedirectToAction("PersonalPage", "Account", new { id = user.Id });
        }
        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> AddItem(int id)
        {
            ItemCollection collection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == id);
            ViewBag.Collection = collection;
            return View();
        }
        [Authorize(Roles = "admin, user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(AddItemModel model)
        {
            ItemCollection collection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == model.ItemCollectionId);
            if (ModelState.IsValid)
            {
                Item item = new Item
                {
                    Title = model.Title,
                    ItemCollectionId = model.ItemCollectionId,
                    NumFields = String.IsNullOrEmpty(collection.NumFields) ? "" : String.Join("|", model.NumFields.ToArray()),
                    StrFields = String.Join("|", model.StrFields.Where(s => !String.IsNullOrWhiteSpace(s)).ToArray()),
                    TextFields = String.Join("|", model.TextFields.Where(s => !String.IsNullOrWhiteSpace(s)).ToArray()),
                    BoolFields = String.Join("|", model.BoolFields.ToArray()),
                    Comments = "",
                    Likes = ""
                };
                collection.Items.Add(item);
                _context.Collections.Update(collection);
                await _context.SaveChangesAsync();
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == collection.OwnerEmail);
                return RedirectToAction("OpenCollection", "Collections", new { id = model.ItemCollectionId });
            }
            ViewBag.Collection = collection;
            return View();
        }
        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> EditItem(int id)
        {
            ItemCollection collection = await _context.Collections.FirstOrDefaultAsync(c => c.Items.Any(i => i.Id == id));
            Item item = collection.Items.FirstOrDefault(i => i.Id == id);
            ViewBag.Collection = collection;
            ViewBag.Item = item;
            return View();
        }
        [Authorize(Roles = "admin, user")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(EditItemModel model)
        {
            ItemCollection collection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == model.ItemCollectionId);
            Item item = collection.Items.FirstOrDefault(i => i.Id == model.Id);
            if (ModelState.IsValid)
            {
                item.Title = model.Title;
                item.NumFields = String.IsNullOrEmpty(collection.NumFields) ? "" : String.Join("|", model.NumFields.ToArray());
                item.StrFields = String.Join("|", model.StrFields.Where(s => !String.IsNullOrWhiteSpace(s)).ToArray());
                item.TextFields = String.Join("|", model.TextFields.Where(s => !String.IsNullOrWhiteSpace(s)).ToArray());
                item.BoolFields = String.Join("|", model.BoolFields.ToArray());
                _context.Collections.Update(collection);
                await _context.SaveChangesAsync();
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == collection.OwnerEmail);
                return RedirectToAction("OpenCollection", "Collections", new { id = model.ItemCollectionId });
            }
            ViewBag.Collection = collection;
            ViewBag.Item = item;
            return View();
        }
        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> DeleteItem(int id)
        {
            ItemCollection collection = await _context.Collections.FirstOrDefaultAsync(c => c.Items.Any(i => i.Id == id));
            Item item = collection.Items.FirstOrDefault(i => i.Id == id);
            collection.Items.Remove(item);
            _context.Collections.Update(collection);
            await _context.SaveChangesAsync();
            return RedirectToAction("OpenCollection", "Collections", new { id = collection.Id });
        }
        [HttpGet]
        public async Task<IActionResult> OpenItem(int id)
        {
            ItemCollection collection = await _context.Collections.FirstOrDefaultAsync(c => c.Items.Any(i => i.Id == id));
            Item item = collection.Items.FirstOrDefault(i => i.Id == id);
            ViewBag.Collection = collection;
            return View(item);
        }
        [Authorize(Roles = "admin, user")]
        [HttpPost]
        public async Task<IActionResult> Comment(int id, string text)
        {
            ItemCollection collection = await _context.Collections.FirstOrDefaultAsync(c => c.Items.Any(i => i.Id == id));
            Item item = collection.Items.FirstOrDefault(i => i.Id == id);
            item.Comments += User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value + " at " + DateTime.Now.ToString() + ": " + 
                text + "|";
            _context.Collections.Update(collection);
            await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        [Authorize(Roles = "admin, user")]
        [HttpGet]
        public async Task<IActionResult> LikeItem(int id)
        {
            string email = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            ItemCollection collection = await _context.Collections.FirstOrDefaultAsync(c => c.Items.Any(i => i.Id == id));
            Item item = collection.Items.FirstOrDefault(i => i.Id == id);
            if (item.Likes.Contains(user.Id.ToString()))
            {
                item.Likes = item.Likes.Replace(user.Id.ToString(), String.Empty);
            }
            else {
                item.Likes += user.Id.ToString();
            }
            _context.Collections.Update(collection);
            await _context.SaveChangesAsync();
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
