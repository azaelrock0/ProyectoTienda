using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Datos.Context;
using Entidades;
using Negocio.Recursos;
using Negocio;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Presentacion.Controllers
{
    public class ComprasController : Controller
    {
        NCliente _nCliente = new NCliente();
        NCompra _nCompra = new NCompra();
        NArticulo _nArticulo = new NArticulo();
        private readonly TiendaContext _context;

        public ComprasController(TiendaContext context)
        {
            _context = context;
        }

        // GET: Compras
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string nombreUsuario = "";
            //string id = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(C=>C.Value).SingleOrDefault();
                //id = claimuser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(C => C.Value).SingleOrDefault();
            }

            ViewData["nombreUsuario"] = nombreUsuario;

              return _context.Articulos != null ? 
                          View(await _context.Articulos.ToListAsync()) :
                          Problem("Entity set 'TiendaContext.Articulos'  is null.");
        }
        [Authorize]
        public async Task<IActionResult> Carrito()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string id = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                id = claimuser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(C => C.Value).SingleOrDefault();
            }

            return View(_nCompra.Consultar(Convert.ToInt32(id)));
        }

        // GET: Compras/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Articulos == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articulo == null)
            {
                return NotFound();
            }
            ClaimsPrincipal claimuser = HttpContext.User;
            string idCliente = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                idCliente = claimuser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(C => C.Value).SingleOrDefault();
            }
            ViewBag.ExisteEnCarrito = _nCompra.ExisteEnCarrito(Convert.ToInt32(idCliente), Convert.ToInt32(id));
            return View(articulo);
        }

        // GET: Compras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int id)
        {

                ClaimsPrincipal claimuser = HttpContext.User;
                string idCliente = "";

                if (claimuser.Identity.IsAuthenticated)
                {
                    idCliente = claimuser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(C => C.Value).SingleOrDefault();
                }
            _nCompra.Agregar(Convert.ToInt32(idCliente), id);
            return RedirectToAction(nameof(Index));
            //return View(_nArticulo.Consultar(id));
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Articulos == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }
            return View(articulo);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Precio,Imagen,Stock")] Articulo articulo)
        {
            if (id != articulo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticuloExists(articulo.Id))
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
            return View(articulo);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Articulos == null)
            {
                return NotFound();
            }

            var articulo = await _context.Articulos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string idCliente = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                idCliente = claimuser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(C => C.Value).SingleOrDefault();
            }
            var clienteArticulo = _nCompra.Consultar(Convert.ToInt32(idCliente), Id);
            if (clienteArticulo != null)
            {
                _nCompra.Eliminar(clienteArticulo);
            }
            return RedirectToAction(nameof(Carrito));
        }
        public RedirectToActionResult DeleteFromCart(int Id)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string idCliente = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                idCliente = claimuser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(C => C.Value).SingleOrDefault();
            }
            var clienteArticulo = _nCompra.Consultar(Convert.ToInt32(idCliente), Id);
            if (clienteArticulo != null)
            {
                _nCompra.Eliminar(clienteArticulo);
            }
            return RedirectToAction(nameof(Carrito));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarCompra()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string idCliente = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                idCliente = claimuser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(C => C.Value).SingleOrDefault();
            }
            _nCompra.ConfirmarCompra(Convert.ToInt32(idCliente));
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string correo, string clave)
        {
            Cliente cliente = _nCliente.Consultar(correo, clave);
            if (cliente == null)
            {
                ViewData["Mensaje"] = "El correo o contraseña es incorrecto.";
                ViewBag.correo = correo;
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, cliente.Nombre),
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(cliente.Id))
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties);

            return RedirectToAction(nameof(Index));
        }

        public async Task< IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }

        private bool ArticuloExists(int id)
        {
          return (_context.Articulos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
