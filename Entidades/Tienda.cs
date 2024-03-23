using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public partial class Tienda
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La sucursal no puede ser vacía.")]
        public string Sucursal { get; set; } = null!;
        public string Direccion { get; set; }
        public IEnumerable<ArticuloTienda> ArticuloTiendas { get; set; }
    }
}
