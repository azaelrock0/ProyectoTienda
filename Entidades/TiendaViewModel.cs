using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class TiendaViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La sucursal no puede ser vacía.")]
        public string Sucursal { get; set; } = null!;
        public string Direccion { get; set; }
        public List<Articulo> Articulos { get; set; }
        public IEnumerable<ArticuloTienda> ArticuloTiendas { get; set; }
    }
}
