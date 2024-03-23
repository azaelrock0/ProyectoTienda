using Datos;
using Datos.Context;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Negocio
{
    public class NTienda
    {
        DRepositorio<Tienda> _DRepositorio = new DRepositorio<Tienda>();
        DRepositorio<ArticuloTienda> _DRepositorioAT = new DRepositorio<ArticuloTienda>();
        DRepositorio<Articulo> _DRepositorioAr = new DRepositorio<Articulo>();
        public List<Tienda> Consultar()
        {
            return _DRepositorio.Consultar();
        }
        public Tienda Consultar(int id)
        {
            return _DRepositorio.Consultas(id);
        }
        public TiendaViewModel ConsultarDetails(int id)
        {
            Tienda tienda = _DRepositorio.Consultas(id);
            IEnumerable<ArticuloTienda> articuloTiendas = _DRepositorioAT.Consultar(x => x.TiendaId == id);
            List<Articulo> articulos = new List<Articulo>();
            foreach (ArticuloTienda articuloTienda in articuloTiendas)
            {
                Articulo articulo = _DRepositorioAr.Consultas(articuloTienda.ArticuloId);
                articulos.Add(articulo);
            }
            var viewModel = new TiendaViewModel
            {
                Id = tienda.Id,
                Sucursal = tienda.Sucursal,
                Direccion = tienda.Direccion,
                Articulos = articulos,
                ArticuloTiendas = articuloTiendas
            };
            return viewModel;
        }
        public void Agregar(Tienda newTienda)
        {
            _DRepositorio.Agregar(newTienda);
        }
        public void Actualizar(Tienda editTienda)
        {
            _DRepositorio.Actualizar(editTienda);
        }
        public void Eliminar(Tienda deleteTienda)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _DRepositorioAT.Eliminar(x => x.TiendaId == deleteTienda.Id);
                    _DRepositorio.Eliminar(deleteTienda);
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
