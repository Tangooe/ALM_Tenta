using ALM_Tenta.Data;
using ALM_Tenta.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ALM_Tenta.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string searchFilter = null)
        {
            if (string.IsNullOrWhiteSpace(searchFilter))
                return View(await _context.Customers.ToListAsync());
            else
                return View(await _context.Customers
                    .Where(c => c.Name.ToLower().Contains(searchFilter.ToLower()))
                    .ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Accounts)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,OrganisationNumber,Name,StreetAddress,City,State,PostalCode,Country,PhoneNumber")]
            Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,OrganisationNumber,Name,StreetAddress,City,State,PostalCode,Country,PhoneNumber")]
            Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Accounts)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            if (customer.Accounts.Sum(a => a.Balance) != 0)
            {
                TempData["Message"] = "Cannot remove a customer with money on their accounts";
                return View(nameof(Index), await _context.Customers.ToListAsync());
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(m => m.Id == id);

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}