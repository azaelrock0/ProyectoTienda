using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Negocio
{
    public class NArticulo
    {
        DRepositorio<Articulo> _DRepositorio = new DRepositorio<Articulo>();
        DRepositorio<ArticuloTienda> _DRepositorioAT = new DRepositorio<ArticuloTienda>();
        DRepositorio<ClienteArticulo> _DRepositorioCA = new DRepositorio<ClienteArticulo>();
        DRepositorio<Tienda> _DRepositorioTi = new DRepositorio<Tienda>();

        public List<Articulo> Consultar()
        {
            return _DRepositorio.Consultar();
        }
        public ArticuloViewModel ConsultarViewModel(int id)
        {
            Articulo articulo = _DRepositorio.Consultas(id);
            List<Tienda> tiendas = _DRepositorioTi.Consultar();
            IEnumerable<ArticuloTienda> articuloTiendas = _DRepositorioAT.Consultar(x => x.ArticuloId == id);
            List<int> tiendasID = articuloTiendas.Select(m => m.TiendaId).ToList();
            var viewModel = new ArticuloViewModel
            {
                Id = articulo.Id,
                Codigo = articulo.Codigo,
                Precio = articulo.Precio,
                Imagen = articulo.Imagen,
                Stock = articulo.Stock,
                Tiendas = tiendas,
                TiendasID = tiendasID,
                ArticuloTiendas = articuloTiendas
            };
            return viewModel;
        }
        public ArticuloViewModel ConsultarDetails(int id)
        {
            Articulo articulo = _DRepositorio.Consultas(id);
            IEnumerable<ArticuloTienda> articuloTiendas = _DRepositorioAT.Consultar(x => x.ArticuloId == id);
            List<Tienda> tiendas = new List<Tienda>();
            foreach (ArticuloTienda articuloTienda in articuloTiendas)
            {
                Tienda tienda = _DRepositorioTi.Consultas(articuloTienda.TiendaId);
                tiendas.Add(tienda);
            }
            List<int> tiendasID = articuloTiendas.Select(m => m.TiendaId).ToList();
            var viewModel = new ArticuloViewModel
            {
                Id = articulo.Id,
                Codigo = articulo.Codigo,
                Precio = articulo.Precio,
                Imagen = articulo.Imagen,
                Stock = articulo.Stock,
                Tiendas = tiendas,
                TiendasID = tiendasID,
                ArticuloTiendas = articuloTiendas
            };
            return viewModel;
        }
        public Articulo Consultar(int id)
        {
            return _DRepositorio.Consultas(id); ;
        }
        public void Agregar(ArticuloViewModel viewModel)
        {
            Articulo articulo = new Articulo()
            {
                Id = viewModel.Id,
                Codigo = viewModel.Codigo,
                Precio = viewModel.Precio,
                Imagen = viewModel.Imagen,
                Stock = viewModel.Stock
            };
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _DRepositorio.Agregar(articulo);
                    DateTime fechaHoy = DateTime.Now;
                    List<ArticuloTienda> articuloTiendas = new List<ArticuloTienda>();
                    foreach (int item in viewModel.TiendasID)
                    {
                        articuloTiendas.Add(new ArticuloTienda()
                        {
                            ArticuloId = articulo.Id,
                            TiendaId = item,
                            Fecha = fechaHoy
                        });
                    }
                    _DRepositorioAT.Agregar(articuloTiendas);
                    scope.Complete();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Actualizar(ArticuloViewModel viewModel)
        {
            Articulo articulo = new Articulo()
            {
                Id = viewModel.Id,
                Codigo = viewModel.Codigo,
                Precio = viewModel.Precio,
                Imagen = viewModel.Imagen,
                Stock = viewModel.Stock
            };
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _DRepositorio.Actualizar(articulo);
                    DateTime fechaHoy = DateTime.Now;
                    List<ArticuloTienda> articuloTiendas = new List<ArticuloTienda>();
                    foreach (int item in viewModel.TiendasID)
                    {
                        articuloTiendas.Add(new ArticuloTienda()
                        {
                            ArticuloId = articulo.Id,
                            TiendaId = item,
                            Fecha = fechaHoy
                        });
                    }
                    _DRepositorioAT.Eliminar(x => x.ArticuloId == articulo.Id);
                    _DRepositorioAT.Agregar(articuloTiendas);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void Eliminar(Articulo articulo)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _DRepositorioAT.Eliminar(x => x.ArticuloId == articulo.Id);
                    _DRepositorioCA.Eliminar(x => x.ArticuloId == articulo.Id);
                    _DRepositorio.Eliminar(articulo);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
