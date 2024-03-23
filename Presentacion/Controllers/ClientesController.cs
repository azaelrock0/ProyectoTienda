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
    public class ClientesController : Controller
    {
        NCliente _nCliente = new NCliente();
        //private readonly TiendaContext _context;

        /*public ClientesController(TiendaContext context)
        {
            _context = context;
        }*/

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
              return View(_nCliente.Consultar());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = _nCliente.Consultar(Convert.ToInt32(id));
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Correo,Nombre,Apellidos,Direccion,Contrasena")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (!_nCliente.CorreoExiste(cliente.Correo))
                {
                    _nCliente.Agregar(cliente);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Correo", $"Este correo ya estpa ocupado. Por favor, ingrese otro.");
                }
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = _nCliente.Consultar(Convert.ToInt32(id));
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Correo,Nombre,Apellidos,Direccion,Contrasena")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _nCliente.Actualizar(cliente);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = _nCliente.Consultar(Convert.ToInt32(id));
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /*if (_context.Clientes == null)
            {
                return Problem("Entity set 'TiendaContext.Clientes'  is null.");
            }*/
            var cliente = _nCliente.Consultar(Convert.ToInt32(id));
            if (cliente != null)
            {
                _nCliente.Eliminar(cliente);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            Cliente existe = _nCliente.Consultar(id);
            if (existe == null)
                return false;
            return true;
        }
    }
}
