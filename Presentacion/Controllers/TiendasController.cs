using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Datos.Context;
using Entidades;
using Negocio;

namespace Presentacion.Controllers
{
    public class TiendasController : Controller
    {
        //private readonly TiendaContext _context;
        NTienda _nTienda = new NTienda();

        /*public TiendasController(TiendaContext context)
        {
            _context = context;
        }*/

        // GET: Tiendas
        public async Task<IActionResult> Index()
        {
            return View(_nTienda.Consultar());
        }

        // GET: Tiendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tienda = _nTienda.ConsultarDetails(Convert.ToInt32(id));
            if (tienda == null)
            {
                return NotFound();
            }

            return View(tienda);
        }

        // GET: Tiendas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tiendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Sucursal,Direccion")] Tienda tienda)
        {
            if (ModelState.IsValid)
            {
                _nTienda.Agregar(tienda);
                return RedirectToAction(nameof(Index));
            }
            return View(tienda);
        }

        // GET: Tiendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tienda = _nTienda.Consultar(Convert.ToInt32(id));
            if (tienda == null)
            {
                return NotFound();
            }
            return View(tienda);
        }

        // POST: Tiendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sucursal,Direccion")] Tienda tienda)
        {
            if (id != tienda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _nTienda.Actualizar(tienda);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TiendaExists(tienda.Id))
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
            return View(tienda);
        }

        // GET: Tiendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tienda = _nTienda.ConsultarDetails(Convert.ToInt32(id));

            if (tienda == null)
            {
                return NotFound();
            }

            return View(tienda);
        }

        // POST: Tiendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /*if (_context.Tiendas == null)
            {
                return Problem("Entity set 'TiendaContext.Tiendas'  is null.");
            }*/
            var tienda = _nTienda.Consultar(id);
            if (tienda != null)
            {
                _nTienda.Eliminar(tienda);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TiendaExists(int id)
        {
            Tienda existe = _nTienda.Consultar(id);
            if (existe == null)
                return false;
            return true;
        }
    }
}
