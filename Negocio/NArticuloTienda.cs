using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class NArticuloTienda
    {
        DRepositorio<ArticuloTienda> _DRepositorio = new DRepositorio<ArticuloTienda>();
        public List<ArticuloTienda> Consultar(int id)
        {
            return _DRepositorio.Consultar(x => x.ArticuloId == id);
        }
    }
}
