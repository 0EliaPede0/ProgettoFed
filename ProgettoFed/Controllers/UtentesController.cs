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
    public class UtentesController : Controller
    {
        private readonly FinaleFedContext _context;

        public UtentesController(FinaleFedContext context)
        {
            _context = context;
        }

        // GET: Utentes
        public async Task<IActionResult> Index()
        {

            if (Request.Cookies["BiscottoUtente"] == null)
            {
                ViewBag.BiscottoUtente = 0;
            }
            else 
            {
                ViewBag.BiscottoUtente = int.Parse(Request.Cookies["BiscottoUtente"]);
            }
              return _context.Utentes != null ? 
                          View(await _context.Utentes.ToListAsync()) :
                          Problem("Entity set 'FinaleFedContext.Utentes'  is null.");
        }

        // GET: Utentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Utentes == null)
            {
                return NotFound();
            }

            var utente = await _context.Utentes
                .FirstOrDefaultAsync(m => m.IdUtente == id);
            if (utente == null)
            {
                return NotFound();
            }

            return View(utente);
        }

        // GET: Utentes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUtente,Nome,Psw,Biscotto")] Utente utente)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in _context.Utentes)
                {
                    if (item.Nome == utente.Nome) 
                    {
                        return View(utente);
                    }
                }

                _context.Add(utente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utente);
        }

        // GET: Utentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Utentes == null)
            {
                return NotFound();
            }

            var utente = await _context.Utentes.FindAsync(id);
            if (utente == null)
            {
                return NotFound();
            }
            return View(utente);
        }

        // POST: Utentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUtente,Nome,Psw,Biscotto")] Utente utente)
        {
            if (id != utente.IdUtente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtenteExists(utente.IdUtente))
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
            return View(utente);
        }

        // GET: Utentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Utentes == null)
            {
                return NotFound();
            }

            var utente = await _context.Utentes
                .FirstOrDefaultAsync(m => m.IdUtente == id);
            if (utente == null)
            {
                return NotFound();
            }

            return View(utente);
        }

        // POST: Utentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Utentes == null)
            {
                return Problem("Entity set 'FinaleFedContext.Utentes'  is null.");
            }
            var utente = await _context.Utentes.FindAsync(id);
            if (utente != null)
            {
                foreach (var item in _context.Nota)
                {
                    if (item.IdUtente == utente.IdUtente) 
                    {
                        item.IdUtente = null;
                    }
                }
                _context.Utentes.Remove(utente);
                
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtenteExists(int id)
        {
          return (_context.Utentes?.Any(e => e.IdUtente == id)).GetValueOrDefault();
        }

        //--------------------------------------------------------------------------------------------------------

        public async Task<IActionResult> CheckCredentialAsync(IFormCollection form)
        {
            string name = form["name"];
            string psw = form["psw"];

            var test = new Utente();
            bool verifica = false;
            foreach (var item in _context.Utentes)
            {
 
                if (item.Nome == name && item.Psw == psw)
                {
                    test = item;
                    verifica = true;
                    break;
                }

            }

            bool verificaBiscottoDoppio = false;
            Random rnd = new Random();
            int casuale = rnd.Next();


            if (verifica)
            {
                while (true)
                {
                    casuale = rnd.Next();
                    foreach (var item in _context.Utentes)
                    {

                        if (item.Biscotto == casuale)
                        {
                            verificaBiscottoDoppio = true;
                            break;
                        }
                    }
                    if (verificaBiscottoDoppio == false)
                    {
                        Response.Cookies.Append("BiscottoUtente", casuale.ToString());
                        test.Biscotto = casuale;
                        break;
                    }
                    verificaBiscottoDoppio = false;
                }


                _context.Update(test);
                await _context.SaveChangesAsync();
                return View("Logged");
            }
            return View("NotLogged");

        }

        public IActionResult LogOut() 
        {
            Response.Cookies.Delete("BiscottoUtente");
            return View("NotLogged");
        }






    }
}
