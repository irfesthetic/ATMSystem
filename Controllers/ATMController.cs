using ATMSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace ATMSystem.Controllers
{
    public class ATMController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ATMController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = _context.Customers
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(customer);
        }

        public IActionResult Balance()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = _context.Customers
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(customer);
        }

        public IActionResult Deposit()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = _context.Customers
            .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(customer);
        }
        [HttpPost]
        public IActionResult DepositMoney(decimal amount)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = _context.Customers
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (amount <= 0)
            {
                ViewBag.Error = "Please enter a valid amount.";
                return View("Deposit", customer);
            }

            decimal previousBalance = customer.Balance;

            customer.Balance += amount;

            var transaction = new ATMSystem.Models.Transaction
            {
                CustomerId = customer.CustomerId,
                TransactionType = "Deposit",
                Amount = amount,
                PreviousBalance = previousBalance,
                NewBalance = customer.Balance,
                Description = "Cash deposit",
                Status = "Success"
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            ViewBag.Success = $"₹{amount:N2} deposited successfully!";

            return View("Deposit", customer);
        }
        public IActionResult Withdraw()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = _context.Customers
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(customer);
        }
        [HttpPost]
        public IActionResult WithdrawMoney(decimal amount)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = _context.Customers
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (amount <= 0)
            {
                ViewBag.Error = "Please enter a valid amount.";
                return View("Withdraw", customer);
            }

            if (amount > customer.Balance)
            {
                ViewBag.Error = "Insufficient balance.";
                return View("Withdraw", customer);
            }

            decimal previousBalance = customer.Balance;

            customer.Balance -= amount;

            var transaction = new ATMSystem.Models.Transaction
            {
                CustomerId = customer.CustomerId,
                TransactionType = "Withdrawal",
                Amount = amount,
                PreviousBalance = previousBalance,
                NewBalance = customer.Balance,
                Description = "Cash withdrawal",
                Status = "Success"
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            ViewBag.Success = $"₹{amount:N2} withdrawn successfully!";

            return View("Withdraw", customer);
        }
        public IActionResult Transactions()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var transactions = _context.Transactions
                .Where(t => t.CustomerId == customerId)
                .OrderByDescending(t => t.TransactionDate)
                .ToList();

            return View(transactions);
        }
        public IActionResult Transfer()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var customer = _context.Customers
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(customer);
        }
        [HttpPost]
        public IActionResult TransferMoney(string receiverAccountNumber, decimal amount)
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            if (customerId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var sender = _context.Customers
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (sender == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (amount <= 0)
            {
                ViewBag.Error = "Please enter a valid amount.";
                return View("Transfer", sender);
            }

            var receiver = _context.Customers
                .FirstOrDefault(c => c.AccountNumber == receiverAccountNumber);

            if (receiver == null)
            {
                ViewBag.Error = "Receiver account not found.";
                return View("Transfer", sender);
            }

            if (receiver.CustomerId == sender.CustomerId)
            {
                ViewBag.Error = "You cannot transfer money to your own account.";
                return View("Transfer", sender);
            }

            if (amount > sender.Balance)
            {
                ViewBag.Error = "Insufficient balance.";
                return View("Transfer", sender);
            }

            decimal senderPreviousBalance = sender.Balance;
            decimal receiverPreviousBalance = receiver.Balance;

            sender.Balance -= amount;
            receiver.Balance += amount;

            var senderTransaction = new ATMSystem.Models.Transaction
            {
                CustomerId = sender.CustomerId,
                TransactionType = "Transfer",
                Amount = amount,
                PreviousBalance = senderPreviousBalance,
                NewBalance = sender.Balance,
                Description = "Money transferred to account " + receiver.AccountNumber,
                Status = "Success"
            };

            var receiverTransaction = new ATMSystem.Models.Transaction
            {
                CustomerId = receiver.CustomerId,
                TransactionType = "Transfer",
                Amount = amount,
                PreviousBalance = receiverPreviousBalance,
                NewBalance = receiver.Balance,
                Description = "Money received from account " + sender.AccountNumber,
                Status = "Success"
            };

            _context.Transactions.Add(senderTransaction);
            _context.Transactions.Add(receiverTransaction);

            _context.SaveChanges();

            ViewBag.Success = $"₹{amount:N2} transferred successfully!";

            return View("Transfer", sender);
        }
    }
}