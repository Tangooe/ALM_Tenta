﻿using System.Collections.Generic;
using ALM_Tenta.Data;
using ALM_Tenta.Models;
using ALM_Tenta.View_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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

        // GET: Accounts/Create
        //public IActionResult Create()
        //{
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id");
        //    return View();
        //}

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,AccountNumber,Balance,CustomerId")] Account account)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(account);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Details), "Customers", new {id = account.CustomerId});
        //    }
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", account.CustomerId);
        //    return View(account);
        //}

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

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.SingleOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", account.CustomerId);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountNumber,Balance,CustomerId")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", account.CustomerId);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            if (account.Balance != 0)
            {
                TempData["Message"] = "Cannot remove an account with money on it";
                return RedirectToAction(nameof(Details), "Customers", new { id = account.CustomerId });
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(m => m.Id == id);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}