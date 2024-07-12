namespace Pizzario.Services.ShoppingCartApi.Models.Dto
{
    public class CartDto
    {
        public CartHeaders CartHeader { get; set; }
        public IEnumerable<CartDetails>? CartDetails { get; set; }
    }
}
