using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Entidades
{
    public partial class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string? Direccion { get; set; }
        [DisplayName("Contraseña")]
        public string Contrasena { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public IEnumerable<ClienteArticulo> ClienteArticulos { get; set; }
    }
}
