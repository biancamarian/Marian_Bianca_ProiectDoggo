using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProiectDoggo.Data;
using ProiectDoggo.Models;

namespace ProiectDoggo.Controllers
{
    [Authorize(Roles="Employee")]
    public class DoggoesController : Controller
    {
        private readonly ShopContext _context;

        public DoggoesController(ShopContext context)
        {
            _context = context;
        }

        // GET: Doggoes
        [AllowAnonymous]
        public async Task<IActionResult> Index(
            string sortOrder, 
            string searchString,
            string currentFilter,
            int? pageNumber
            )
        {
            ViewData["BreedSortParm"] = String.IsNullOrEmpty(sortOrder) ? "breed_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            ViewData["CurrentSort"] = sortOrder;

            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var doggoes = from d in _context.Doggoes
                        select d;

            if (!String.IsNullOrEmpty(searchString))
            {
                doggoes = doggoes.Where(s => s.Breed.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "breed_desc":
                    doggoes = doggoes.OrderByDescending(b => b.Breed);
                    break;
                case "Price":
                    doggoes = doggoes.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    doggoes = doggoes.OrderByDescending(b => b.Price);
                    break;
                default:
                    doggoes = doggoes.OrderBy(b => b.Breed);
                    break;
            }

            int pageSize = 3;

            return View(await PaginatedList<Doggo>.CreateAsync(doggoes.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Doggoes/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           /* var doggo = await _context.Doggoes
                .Include(s => s.Orders)
                .ThenInclude(e => e.Customer)
                 .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DoggoID == id);*/
            var doggo = await _context.Doggoes
                .Include(s => s.KennelDoggoes)
                .ThenInclude(e => e.Kennel)
                 .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DoggoID == id);
            if (doggo == null)
            {
                return NotFound();
            }

            return View(doggo);
        }

        // GET: Doggoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Doggoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Breed,Color,Gender,Price")] Doggo doggo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(doggo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException/* ex*/)
            {

                ModelState.AddModelError("", "Nu s-a putut efectua schimbare " + "Incearca iar ");
            }
            return View(doggo);
        }

        // GET: Doggoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doggo = await _context.Doggoes.FindAsync(id);
            if (doggo == null)
            {
                return NotFound();
            }
            return View(doggo);
        }

        // POST: Doggoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doggoToUpdate = await _context.Doggoes.FirstOrDefaultAsync(s => s.DoggoID == id);
            if (await TryUpdateModelAsync<Doggo>(
            doggoToUpdate,
            "",
             s => s.Color, s => s.Gender, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Nu s-a putut efectua schimbare " + "Incearca iar ");
                }
            }
            return View(doggoToUpdate);

        }

        // GET: Doggoes/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError=false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doggo = await _context.Doggoes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DoggoID == id);
            if (doggo == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =  "Stergerea a esuat. Mai incearca";
            }

            return View(doggo);
        }

        // POST: Doggoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doggo = await _context.Doggoes.FindAsync(id);

            if (doggo == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Doggoes.Remove(doggo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }catch (DbUpdateException /* ex */)
            {
               return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool DoggoExists(int id)
        {
            return _context.Doggoes.Any(e => e.DoggoID == id);
        }
    }
}
