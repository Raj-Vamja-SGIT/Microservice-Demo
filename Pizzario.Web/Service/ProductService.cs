using Pizzario.Web.Models;
using Pizzario.Web.Service.IService;
using Pizzario.Web.Utility;

namespace Pizzario.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto> CreateProductAsync(Products product)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = product,
                Url = StaticDetails.ProductApiBase + "/api/ProductAPI"
            });
        }

        public async Task<ResponseDto> DeleteProductAsync(int productId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.ProductApiBase + "/api/ProductAPI/" + productId
            });
        }

        public async Task<ResponseDto> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ProductApiBase + "/api/ProductAPI"
            });
        }
        public async Task<ResponseDto> GetProductByCategoryAsync(string category)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ProductApiBase + "/api/ProductAPI/GetProductByCategory/" + category
            });
        }

        public async Task<ResponseDto> GetProductByIdAsync(int productId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ProductApiBase + "/api/ProductAPI/" + productId
            });
        }

        public async Task<ResponseDto> UpdateProductAsync(Products product)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = product,
                Url = StaticDetails.ProductApiBase + "/api/ProductAPI"
            });
        }
    }
}
