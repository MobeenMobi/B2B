using B2B.Data; // Replace with your DbContext namespace
using B2B.Models; // Replace with your Models namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace B2B.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly ApplicationDbContext _context; // Replace with your DbContext

        public UserManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserManagement
        public async Task<IActionResult> Index()
        {
            var users = await _context.OTRUsers.ToListAsync();
            return View(users);
        }

        // GET: UserManagement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.OTRUsers.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        // GET: UserManagement/Create
        public IActionResult Create()
        {
            var roles = _context.Roles
            .Where(r => r.IsActive) 
            .Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.RoleName
            })
            .ToList();


            ViewBag.Roles = roles;
            return View();
        }

        // POST: UserManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OTRUsers user)
        {
            if (ModelState.IsValid)
            {
                user.CreatedAt = DateTime.Now;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: UserManagement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.OTRUsers.FindAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: UserManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OTRUsers user)
        {
            if (id != user.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: UserManagement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.OTRUsers.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: UserManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.OTRUsers.FindAsync(id);
            if (user != null)
            {
                _context.OTRUsers.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.OTRUsers.Any(e => e.Id == id);
        }
    }
}
