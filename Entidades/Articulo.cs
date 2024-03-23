using System;
using System.Collections.Generic;

namespace Entidades
{
    public partial class Articulo
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public int Precio { get; set; }
        public string? Imagen { get; set; }
        public int Stock { get; set; }
        public IEnumerable<ArticuloTienda> ArticuloTiendas { get; set; }
        public IEnumerable<ClienteArticulo> ClienteArticulos { get; set; }
    }
}
