using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProgettoFed.Models;

namespace ProgettoFed.Controllers
{
    public class NotumsController : Controller
    {
        private readonly FinaleFedContext _context;

        public NotumsController(FinaleFedContext context)
        {
            _context = context;
        }

        // GET: Notums
        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["BiscottoUtente"] != null)
            {
                int biscotto = int.Parse(Request.Cookies["BiscottoUtente"]);
                foreach (var item in _context.Utentes)
                {
                    if (item.Biscotto == biscotto)
                    {
                        ViewBag.idUtente = item.IdUtente;
                        break;
                    }
                }
                var finaleFedContext = _context.Nota.Include(n => n.IdUtenteNavigation);
                return View(await finaleFedContext.ToListAsync());
            }

            return View("Views/Home/Index.cshtml");


        }

        // GET: Notums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Nota == null)
            {
                return NotFound();
            }

            var notum = await _context.Nota
                .Include(n => n.IdUtenteNavigation)
                .FirstOrDefaultAsync(m => m.IdTask == id);
            if (notum == null)
            {
                return NotFound();
            }

            return View(notum);
        }

        // GET: Notums/Create
        public IActionResult Create()
        {
            ViewData["IdUtente"] = new SelectList(_context.Utentes, "IdUtente", "IdUtente");
            return View();
        }

        // POST: Notums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTask,IdUtente,DataCreazione,DataScadenza,TitoloTask,TestoTask,Flag")] Notum notum)
        {
            if (ModelState.IsValid)
            {
                int biscotto = int.Parse(Request.Cookies["BiscottoUtente"]);
                int idUtente = 0;
                foreach (var item in _context.Utentes)
                {
                    if (item.Biscotto == biscotto)
                    {
                        idUtente = item.IdUtente;
                        break;
                    }
                }
                notum.DataCreazione = DateTime.Now;
                notum.IdUtente = idUtente;
                _context.Add(notum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUtente"] = new SelectList(_context.Utentes, "IdUtente", "IdUtente", notum.IdUtente);
            return View(notum);
        }

        // GET: Notums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Nota == null)
            {
                return NotFound();
            }

            var notum = await _context.Nota.FindAsync(id);
            if (notum == null)
            {
                return NotFound();
            }
            ViewData["IdUtente"] = new SelectList(_context.Utentes, "IdUtente", "IdUtente", notum.IdUtente);
            return View(notum);
        }

        // POST: Notums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTask,IdUtente,DataCreazione,DataScadenza,TitoloTask,TestoTask,Flag")] Notum notum)
        {
            if (id != notum.IdTask)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotumExists(notum.IdTask))
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
            ViewData["IdUtente"] = new SelectList(_context.Utentes, "IdUtente", "IdUtente", notum.IdUtente);
            return View(notum);
        }

        // GET: Notums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Nota == null)
            {
                return NotFound();
            }

            var notum = await _context.Nota
                .Include(n => n.IdUtenteNavigation)
                .FirstOrDefaultAsync(m => m.IdTask == id);
            if (notum == null)
            {
                return NotFound();
            }

            return View(notum);
        }

        // POST: Notums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Nota == null)
            {
                return Problem("Entity set 'FinaleFedContext.Nota'  is null.");
            }
            var notum = await _context.Nota.FindAsync(id);
            if (notum != null)
            {
                _context.Nota.Remove(notum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotumExists(int id)
        {
          return (_context.Nota?.Any(e => e.IdTask == id)).GetValueOrDefault();
        }
//-----------------------------------------------------------------------------------------------
        public async Task<IActionResult> RicercaFiltrata(IFormCollection form)
        {
            string name = form["Search"];
            ViewBag.FiltroContenuto = name;
            if (Request.Cookies["BiscottoUtente"] != null)
            {
                int biscotto = int.Parse(Request.Cookies["BiscottoUtente"]);
                foreach (var item in _context.Utentes)
                {
                    if (item.Biscotto == biscotto)
                    {
                        ViewBag.idUtente = item.IdUtente;
                        break;
                    }
                }
                var finaleFedContext = _context.Nota.Include(n => n.IdUtenteNavigation);
                return View(await finaleFedContext.ToListAsync());
            }

            return View("Views/Home/Index.cshtml");



        }

        public async Task<IActionResult> Pulisci()
        {
            foreach (var item in _context.Nota)
            {
                if (item.IdUtente == null)
                {
                    _context.Nota.Remove(item);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
