using ALM_Tenta.Data;
using ALM_Tenta.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ALM_Tenta.ViewModels;

namespace ALM_Tenta.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Accounts.Include(a => a.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.Customer)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int customerId)
        {
            _context.Add(new Account
            {
                Balance = 0,
                CustomerId = customerId
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), "Customers", new { id = customerId });
        }

        // POST: Accounts/Delete/5
        [HttpPost] 
        public async Task<IActionResult> Delete(int accountNumber)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(m => m.AccountNumber == accountNumber);
            var routeId = account.CustomerId;

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), "Customers", new { id = routeId });
        }

        [HttpGet]
        public async Task<IActionResult> Deposit(int customerId)
        {
            var accounts = await _context.Accounts.Where(a => a.CustomerId == customerId).ToListAsync();

            return View(new DepositViewModel
            {
                Accounts = new SelectList(accounts, "Id", "Name"),
                Amount = 0
            });
        }
        [HttpPost]
        public async Task<IActionResult> Deposit(DepositViewModel model)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(m => m.Id == model.Id);
            account.Deposit(model.Amount);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{model.Amount} has been deposited to account {account.AccountNumber}";
            return RedirectToAction(nameof(Details), "Customers", new { id = account.CustomerId });
        }

        [HttpGet]
        public async Task<IActionResult> Withdrawal(int customerId)
        {
            var accounts = await _context.Accounts.Where(a => a.CustomerId == customerId).ToListAsync();

            return View(new DepositViewModel
            {
                Accounts = new SelectList(accounts, "Id", "Name"),
                Amount = 0
            });
        }
        [HttpPost]
        public async Task<IActionResult> Withdrawal(DepositViewModel model)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(m => m.Id == model.Id);

            if (account.Withdrawal(model.Amount))
            {
                await _context.SaveChangesAsync();

                TempData["Message"] = $"{model.Amount} has been withdrawn from account {account.AccountNumber}";
                return RedirectToAction(nameof(Details), "Customers", new { id = account.CustomerId });
            }

            TempData["Message"] = $"Failed to withdraw {model.Amount}:- from account {account.AccountNumber}";
            return RedirectToAction(nameof(Details), "Customers", new { id = account.CustomerId });
        }

        [HttpGet]
        public async Task<IActionResult> Transfer(int customerId)
        {
            var accounts = await _context.Accounts.Where(a => a.CustomerId == customerId).ToListAsync();

            return View(new TransferViewModel
            {
                SenderAccounts = new SelectList(accounts, "Id", "Name"),
                ReciepentAccounts = new SelectList(await _context.Accounts.ToListAsync(), "Id", "Name"),
                Amount = 0
            });
        }
        [HttpPost]
        public async Task<IActionResult> Transfer(TransferViewModel model)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(m => m.Id == model.SenderAccountId);

            if (account.Withdrawal(model.Amount))
            {
                var reciepentAccount =
                    await _context.Accounts.SingleOrDefaultAsync(a => a.Id == model.ReciepentAccountId);

                reciepentAccount.Deposit(model.Amount);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"{model.Amount} has been transfered from account {account.AccountNumber} to account {reciepentAccount.AccountNumber}";
                return RedirectToAction(nameof(Details), "Customers", new { id = account.CustomerId });
            }

            TempData["Message"] = $"Failed to withdraw {model.Amount}:- from account {account.AccountNumber}";
            return RedirectToAction(nameof(Details), "Customers", new { id = account.CustomerId });
        }
    }
}
