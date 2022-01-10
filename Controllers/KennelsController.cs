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
using ProiectDoggo.Models.ShopViewModels;
using Microsoft.AspNetCore.Identity;


namespace ProiectDoggo.Controllers
{
    [Authorize(Policy = "OnlySales")]

    public class KennelsController : Controller
    {
        private readonly ShopContext _context;

        public KennelsController(ShopContext context)
        {
            _context = context;
        }

        // GET: Kennels
        public async Task<IActionResult> Index(int? id, int? doggoID)
        {
            var viewModel = new KennelIndexData();
            viewModel.Kennels = await _context.Kennels
            .Include(i => i.KennelDogs)
            .ThenInclude(i => i.Doggo)
            .ThenInclude(i => i.Orders)
            .ThenInclude(i => i.Customer)
            .AsNoTracking()
            .OrderBy(i => i.KennelName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["KennelID"] = id.Value;
                Kennel kennel = viewModel.Kennels.Where(
                i => i.KennelID == id.Value).Single();
                viewModel.Doggoes = kennel.KennelDogs.Select(s => s.Doggo);
            }
            if (doggoID != null)
            {
                ViewData["DoggoID"] = doggoID.Value;
                viewModel.Orders = viewModel.Doggoes.Where(
                x => x.DoggoID == doggoID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Kennels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kennel = await _context.Kennels
                .Include(s => s.KennelDogs)
                .ThenInclude(e => e.Doggo)
                 .AsNoTracking()
                .FirstOrDefaultAsync(m => m.KennelID == id);
            if (kennel == null)
            {
                return NotFound();
            }

            return View(kennel);
        }

        // GET: Kennels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kennels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KennelID,KennelName,Address")] Kennel kennel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kennel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kennel);
        }

        // GET: Kennels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kennel = await _context.Kennels
                .Include(i => i.KennelDogs)
                .ThenInclude(i => i.Doggo)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.KennelID == id);
            if (kennel == null)
            {
                return NotFound();
            }
            PopulateKennelDoggoData(kennel);
            return View(kennel);
        }

        private void PopulateKennelDoggoData(Kennel kennel)
        {
            var allDoggoes = _context.Doggoes;
            var kennelDoggoes = new HashSet<int>(kennel.KennelDogs.Select(c => c.DoggoID));
            var viewModel = new List<KennelDoggoData>();
            foreach (var doggo in allDoggoes)
            {
                viewModel.Add(new KennelDoggoData
                {
                    DogggoID = doggo.DoggoID,
                    Breed = doggo.Breed,
                    IsPublished = kennelDoggoes.Contains(doggo.DoggoID)
                });
            }
            ViewData["Doggoes"] = viewModel;
        }


        // POST: Kennels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedDoggoes)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kennelToUpdate = await _context.Kennels
                .Include(i => i.KennelDogs)
                .ThenInclude(i => i.Doggo)
                .FirstOrDefaultAsync(m => m.KennelID == id);

            if(await TryUpdateModelAsync<Kennel>(
                kennelToUpdate,
                "",
                i=>i.KennelName, i=>i.Address))
            {
                UpdateKennelDoggoes(selectedDoggoes, kennelToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateException /* ex*/)
                {
                    ModelState.AddModelError("", "Schimbarile nu au putut fi salvate" + "Mai incearca ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateKennelDoggoes(selectedDoggoes, kennelToUpdate);
            PopulateKennelDoggoData(kennelToUpdate);
            return View(kennelToUpdate);
        }

        private void UpdateKennelDoggoes(string[] selectedDoggoes, Kennel kennelToUpdate)
        {
            if (selectedDoggoes == null)
            {
                kennelToUpdate.KennelDogs = new List<KennelDoggo>();
                return;
            }

            var selectedDoggoesHS = new HashSet<string>(selectedDoggoes);
            var kennelDoggoes = new HashSet<int>
                (kennelToUpdate.KennelDogs.Select(c => c.Doggo.DoggoID));
            foreach(var doggo in _context.Doggoes)
            {
                if (selectedDoggoesHS.Contains(doggo.DoggoID.ToString()))
                {
                    if (!kennelDoggoes.Contains(doggo.DoggoID))
                    {
                        kennelToUpdate.KennelDogs.Add(new KennelDoggo
                        {
                            KennelID = kennelToUpdate.KennelID,
                            DoggoID = doggo.DoggoID
                        });
                    }
                }
                else
                {
                    if (kennelDoggoes.Contains(doggo.DoggoID))
                    {
                        KennelDoggo doggoToRemove = kennelToUpdate.KennelDogs
                            .FirstOrDefault(i => i.DoggoID == doggo.DoggoID);
                        _context.Remove(doggoToRemove);
                    }
                }
            }
        }

        // GET: Kennels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kennel = await _context.Kennels
                .FirstOrDefaultAsync(m => m.KennelID == id);
            if (kennel == null)
            {
                return NotFound();
            }

            return View(kennel);
        }

        // POST: Kennels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kennel = await _context.Kennels.FindAsync(id);
            _context.Kennels.Remove(kennel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KennelExists(int id)
        {
            return _context.Kennels.Any(e => e.KennelID == id);
        }
    }
}
