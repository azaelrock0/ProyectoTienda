using System;
using System.Collections.Generic;

namespace Entidades
{
    public partial class ArticuloTienda
    {
        public int ArticuloId { get; set; }
        public int TiendaId { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Articulo Articulo { get; set; } = null!;
        public virtual Tienda Tienda { get; set; } = null!;
    }
}
