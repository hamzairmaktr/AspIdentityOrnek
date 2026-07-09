using System.Security.Claims;
using AspIdentityOrnek.Data;
using AspIdentityOrnek.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private readonly AppDbContext _context;

        public ToDoController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<ToDoItem> items;
            if (User.IsInRole("Admin"))
                items = await _context.ToDoItems.Include(t => t.User).ToListAsync();
            else
                items = await _context.ToDoItems.Where(t => t.UserId == userId).ToListAsync();
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                var item = new ToDoItem
                {
                    Title = title,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };
                _context.ToDoItems.Add(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.ToDoItems.FindAsync(id);
            if (item != null && (User.IsInRole("Admin") || item.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                _context.ToDoItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}