using Pizzario.Web.Models;

namespace Pizzario.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto> GetProductByCategoryAsync(string category);
        Task<ResponseDto> GetAllProductsAsync();
        Task<ResponseDto> GetProductByIdAsync(int productId);
        Task<ResponseDto> CreateProductAsync(Products product);
        Task<ResponseDto> UpdateProductAsync(Products product);
        Task<ResponseDto> DeleteProductAsync(int productId);
    }
}
