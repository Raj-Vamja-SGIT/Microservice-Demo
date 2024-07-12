using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using Pizzario.Services.ShoppingCartApi.Models.Dto;

namespace Pizzario.Services.ShoppingCartApi.Models
{
    public class CartDetails
    {
        [Key]
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public CartHeaders? CartHeader { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public Product? Product { get; set; }
        public int Count { get; set; }
    }
}
