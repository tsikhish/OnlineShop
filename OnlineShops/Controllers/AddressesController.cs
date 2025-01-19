using Microsoft.AspNetCore.Mvc;
using OnlineShops.Models;

namespace OnlineShops.Controllers
{
    public class AddressesController : Controller
    {
        private readonly AdventureWorksLT2019Context _context;

        public AddressesController(AdventureWorksLT2019Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var addresses = _context.Addresses.ToList();
            return View(addresses);
        }

        public IActionResult Details(int id)
        {
            var address = _context.Addresses.FirstOrDefault(a => a.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Addresses.Add(address);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(address);

        }

        public IActionResult Edit(int id)
        {
            var address = _context.Addresses.FirstOrDefault(a => a.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Address address)
        {
            if (id != address.AddressId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Addresses.Update(address);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }

        public IActionResult Delete(int id)
        {
            var address = _context.Addresses.FirstOrDefault(a => a.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var address = _context.Addresses.FirstOrDefault(a => a.AddressId == id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

