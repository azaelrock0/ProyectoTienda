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
using Presentacion.Models;
using Microsoft.AspNetCore.Hosting;

namespace Presentacion.Controllers
{
    public class ArticulosController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        NTienda _NTienda = new NTienda();
        NArticulo _NArticulo = new NArticulo();
        NArticuloTienda _NArticuloTienda = new NArticuloTienda();

        public ArticulosController( IWebHostEnvironment hostEnvironment)
        {
            this.webHostEnvironment = hostEnvironment;
        }

        // GET: Articulos
        public async Task<IActionResult> Index()
        {
            return View(_NArticulo.Consultar());
        }

        // GET: Articulos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = _NArticulo.ConsultarDetails(Convert.ToInt32(id));
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // GET: Articulos/Create
        public IActionResult Create()
        {
            List<Tienda> tiendas = _NTienda.Consultar();
            //ViewBag.Tiendas = new SelectList(tiendas, "ID", "Sucursal");
            //ViewBag.Tiendas = _NTienda.Consultar();
            //ViewBag.Tiendas = tiendas;
            var viewModel = new ArticuloViewModel
            {
                Tiendas = tiendas,
                //TiendasID = tiendas.Select(m => m.Id).ToList()
            };
            return View(viewModel);
        }

        // POST: Articulos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Precio,ImageFile,ImageName,Stock,TiendasID")] ArticuloViewModel articulo)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {

                string uniqueFileName = UploadedFile(articulo);
                articulo.Imagen = uniqueFileName;
                try
                {
                    _NArticulo.Agregar(articulo);
                }catch(Exception e){
                    if (uniqueFileName != null)
                    {
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                        string imagePath = uploadsFolder + "/" + uniqueFileName;
                        System.IO.File.Delete(imagePath);
                    }
                }
                
                return RedirectToAction(nameof(Index));
            }
            List<Tienda> tiendas = _NTienda.Consultar();
            articulo.Tiendas = tiendas;
            return View(articulo);
        }

        // GET: Articulos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = _NArticulo.ConsultarViewModel(Convert.ToInt32(id));
            articulo.OldImage = articulo.Imagen;
            if (articulo == null)
            {
                return NotFound();
            }
            return View(articulo);
        }

        // POST: Articulos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Precio,ImageFile,ImageName,OldImage,Stock,TiendasID")] ArticuloViewModel articulo)
        {
            //var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (id != articulo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string oldImageName = articulo.OldImage;
                string uniqueFileName = UploadedFile(articulo);
                if (uniqueFileName != null)
                    articulo.Imagen = uniqueFileName;
                else
                    articulo.Imagen = oldImageName;
                try
                {
                    _NArticulo.Actualizar(articulo);
                    if(oldImageName != null && articulo.Imagen != oldImageName)
                    {
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                        string imagePath = uploadsFolder + "/" + oldImageName;
                        System.IO.File.Delete(imagePath);
                    }
                }
                catch (Exception ex)
                {
                    if (uniqueFileName != null)
                    {
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                        string imagePath = uploadsFolder + "/" + uniqueFileName;
                        System.IO.File.Delete(imagePath);
                    }
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
            List<Tienda> tiendas = _NTienda.Consultar();
            articulo.Tiendas = tiendas;
            return View(articulo);
        }

        // GET: Articulos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articulo = _NArticulo.ConsultarDetails(Convert.ToInt32(id));
            if (articulo == null)
            {
                return NotFound();
            }

            return View(articulo);
        }

        // POST: Articulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /*if (_context.Articulos == null)
            {
                return Problem("Entity set 'TiendaContext.Articulos'  is null.");
            }*/
            var articulo = _NArticulo.Consultar(id);
            string imageName = "";
            if (articulo != null)
            {
                imageName = articulo.Imagen;
                _NArticulo.Eliminar(articulo);
            }
            //Confirmar si se eliminó el artículo para eliminar la imagen
            articulo = _NArticulo.Consultar(id);
            if(articulo == null && imageName != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                string imagePath = uploadsFolder + "/" + imageName;
                System.IO.File.Delete(imagePath);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ArticuloExists(int id)
        {
            //return (_context.Articulos?.Any(e => e.Id == id)).GetValueOrDefault();
            Articulo existe = _NArticulo.Consultar(id);
            if (existe == null)
                return false;
            return true;
        }

        private string UploadedFile(ArticuloViewModel model)
        {
            string uniqueFileName = null;

            if (model.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
