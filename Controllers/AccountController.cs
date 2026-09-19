using ATMSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace ATMSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string accountNumber, string pin)
        {
            var customer = _context.Customers
                .FirstOrDefault(c =>
                    c.AccountNumber == accountNumber &&
                    c.Pin == pin &&
                    c.IsActive);

            if (customer == null)
            {
                ViewBag.Error = "Invalid account number or PIN.";
                return View();
            }

            HttpContext.Session.SetInt32("CustomerId", customer.CustomerId);

            return RedirectToAction("Index", "ATM");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}