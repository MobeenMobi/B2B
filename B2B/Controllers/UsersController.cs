using B2B.Data;
using B2B.EmailService;
using B2B.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace B2B.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public UsersController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
            string Email = _context.UserLogins.Where(x => x.UserId == id).Select(y => y.Email).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            _context.SaveChanges();


            user.IsKYBApproved = true;
            _emailService.SendEmailAsync(Email, "KYB Approved", "Congratulations your KYB has been approved.");

            TempData["ApproveMessage"] = "User has been Approved successfully.";

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
                }).ToList();

            return Json(documents);
        }

        [HttpGet]
        public IActionResult KYBReturn(int id, string comments)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            string Email = _context.UserLogins.Where(x => x.UserId == id).Select(y => y.Email).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            user.ReturnComments = comments;
            user.IsReturned = true;
            _context.SaveChanges();

            _emailService.SendEmailAsync(Email, "Application Returned", comments);

            TempData["ReturnMessage"] = "User has been returned successfully.";

            return RedirectToAction("UsersList");
        }

    }
}
