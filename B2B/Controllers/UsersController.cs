using B2B.Data;
using Microsoft.AspNetCore.Mvc;

namespace B2B.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UsersList()
        {
            var users = _context.Users.ToList();

            return View(users);
        }


        public IActionResult UserEdit(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            user.IsKYBApproved = true;
            _context.SaveChanges();

            return RedirectToAction("UserList");
        }



    }
}
