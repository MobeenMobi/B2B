using B2B.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace B2B.Controllers
{
    public class Transaction : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult SinglePayment()
        {
            BusinessPaymentViewModel model = new BusinessPaymentViewModel();

            return View(model);
        }
    }
}
