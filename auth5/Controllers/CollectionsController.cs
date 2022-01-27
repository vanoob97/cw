using auth5.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace auth5.Controllers
{
    public class CollectionsController : Controller
    {
        private ApplicationContext _context;
        public CollectionsController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult AddCollection(int id)
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> OpenCollection(int id)
        {
            ItemCollection collection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == id);
            return View(collection);
        }
    }
}
