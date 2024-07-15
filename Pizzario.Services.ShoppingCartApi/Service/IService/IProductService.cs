using Pizzario.Services.ShoppingCartApi.Models.Dto;

namespace Pizzario.Services.ShoppingCartApi.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();
    }
}
