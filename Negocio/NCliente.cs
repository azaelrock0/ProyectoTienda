using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Negocio.Recursos;
using Microsoft.EntityFrameworkCore;

namespace Negocio
{
    public class NCliente
    {
        DRepositorio<Cliente> _DRepositorio = new DRepositorio<Cliente>();
        DRepositorio<ClienteArticulo> _DRepositorioCA = new DRepositorio<ClienteArticulo>();
        public List<Cliente> Consultar()
        {
            return _DRepositorio.Consultar();
        }
        public Cliente Consultar(int id)
        {
            return _DRepositorio.Consultas(id);
        }
        public Cliente Consultar(string correo, string contrasena)
        {
            return _DRepositorio.Consultas(x => x.Correo == correo && x.Contrasena == Utilidades.EncriptarClave(contrasena));
        }
        public void Agregar(Cliente newCliente)
        {
                newCliente.Contrasena = Utilidades.EncriptarClave(newCliente.Contrasena);
                _DRepositorio.Agregar(newCliente);
        }
        public void Actualizar(Cliente editCliente)
        {
            _DRepositorio.Actualizar(editCliente);
        }
        public void Eliminar(Cliente deleteCliente)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _DRepositorioCA.Eliminar(x => x.ClienteId == deleteCliente.Id);
                    _DRepositorio.Eliminar(deleteCliente);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        public bool CorreoExiste(string correo)
        {
            Cliente cliente = _DRepositorio.Consultas(x => x.Correo == correo);
            if (cliente != null)
                return true;
            else
                return false;
        }
    }
}
