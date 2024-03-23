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
    public class NCompra
    {
        DRepositorio<ClienteArticulo> _DRepositorio = new DRepositorio<ClienteArticulo>();
        DRepositorio<Articulo> _DRepositorioAr = new DRepositorio<Articulo> ();

        public ClienteArticulo Consultar(int idCliente, int idArticulo)
        {
            return _DRepositorio.Consultas(x => x.ClienteId == idCliente && x.ArticuloId == idArticulo);
        }
        public List<Articulo> Consultar(int id)
        {
            IEnumerable<ClienteArticulo> clienteArticulos = _DRepositorio.Consultar(x => x.ClienteId == id);
            List<Articulo> articulos = new List<Articulo>();
            foreach(ClienteArticulo clienteArticulo in clienteArticulos)
            {
                Articulo articulo = _DRepositorioAr.Consultas(clienteArticulo.ArticuloId);
                articulos.Add(articulo);
            }
            return articulos;
        }

        public void Agregar(int idCliente, int idArticulo)
        {
            ClienteArticulo clienteArticulo = new ClienteArticulo()
            {
                ClienteId = idCliente,
                ArticuloId = idArticulo,
                Fecha = DateTime.Now
            };
            _DRepositorio.Agregar(clienteArticulo);
        }
        public void Eliminar(ClienteArticulo clienteArticulo)
        {
            _DRepositorio.Eliminar(clienteArticulo);
        }
        public void ConfirmarCompra(int idCliente)
        {
            IEnumerable<ClienteArticulo> clienteArticulos = _DRepositorio.Consultar(x => x.ClienteId == idCliente);
            List<Articulo> articulos = new List<Articulo>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (ClienteArticulo clienteArticulo in clienteArticulos)
                    {
                        Articulo articulo = _DRepositorioAr.Consultas(clienteArticulo.ArticuloId);
                        if (articulo.Stock > 0)
                            articulo.Stock--;
                        _DRepositorioAr.Actualizar(articulo);
                    }
                    _DRepositorio.Eliminar(x => x.ClienteId == idCliente);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public bool ExisteEnCarrito(int idCliente, int idArticulo)
        {
            ClienteArticulo clienteArticulo = _DRepositorio.Consultas(x => x.ClienteId == idCliente && x.ArticuloId == idArticulo);
            if(clienteArticulo != null)
                return true;
            return false;
        }
    }
}
