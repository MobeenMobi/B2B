using B2B.Data;
using B2B.Models; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Create(OTRUsers user)
        {
            if (ModelState.IsValid)
            {
                user.CreatedAt = DateTime.Now;
                user.IsActive = true;
                _context.Add(user);
                _context.SaveChanges();

                int otrUserId = _context.OTRUsers.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();

                var userLogin = new UserLogins
                {
                    UserId = otrUserId,
                    Email = user.Email,
                    PasswordHash = user.Password,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    LastLogin = null,
                    OTP = "",
                    OTPCreatedAt = null,
                    RoleId = user.RoleId,
                    IsOTRUser = true
                    
                };


                _context.Add(userLogin);
                _context.SaveChanges();
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
            OTRUsers otrUser = _context.OTRUsers.Where(x => x.Id == user.Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
                {
                    otrUser.FirstName = user.FirstName;
                    otrUser.LastName = user.LastName;   
                    otrUser.MiddleName = user.MiddleName;
                    otrUser.IDType = user.IDType;
                    otrUser.IDNumber = user.IDNumber;
                    otrUser.PhoneNumber = user.PhoneNumber;
                    otrUser.Email = user.Email;


                    _context.Update(otrUser);
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
