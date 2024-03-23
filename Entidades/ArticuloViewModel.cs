using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Entidades
{
    public class ArticuloViewModel
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public int Precio { get; set; }
        public string? Imagen { get; set; }
        public int Stock { get; set; }
        public IEnumerable<ArticuloTienda> ArticuloTiendas { get; set; }
        public List<Tienda> Tiendas { get; set; }
        [DisplayName("Tiendas")]
        [Required(ErrorMessage = "Se debe seleccionar al menos una tienda.")]
        public List<int> TiendasID { get; set; }
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png" })]
        [MaxFileSize(5 * 1024 * 1024)]
        [DisplayName("Imagen")]
        public IFormFile ImageFile { get; set; }
        [StringLength(50, ErrorMessage = "El nombre de la imagen no puede tener más de 50 carácteres.")]
        public string ImageName { get; set; }
        public string OldImage {  get; set; }
    }
}
