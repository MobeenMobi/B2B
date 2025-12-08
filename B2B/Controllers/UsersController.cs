using B2B.Data;
using B2B.ViewModels;
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

            var userCompanyDetails =
            from u in _context.Users
            join c in _context.CompanyInfo
                on u.Id equals c.UserId
            select new UserCompanyDetailsViewModel
            {
                // User fields
                UserId = u.Id,
               FirstName = u.FirstName,
               LastName = u.LastName,
               IsKYBApproved = u.IsKYBApproved,

                // CompanyInfo fields
                CompanyName = c.CompanyName,
                BusinessType = c.BusinessType

             };


            return View(userCompanyDetails);
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

            return RedirectToAction("UsersList");
        }

        public IActionResult GetUserDocuments(int userId)
        {
            var documents = _context.Documents
                .Where(d => d.UserId == userId)
                .Select(d => new
                {
                    documentName = d.DocumentName,
                    documentPath = Url.Content("~/uploads/" + Path.GetFileName(d.DocumentPath))
                    //documentPath = d.DocumentPath   
                }).ToList();

            return Json(documents);
        }


    }
}
