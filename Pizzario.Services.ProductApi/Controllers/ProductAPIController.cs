using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pizzario.Services.ProductApi.Data;
using Pizzario.Services.ProductApi.Models;
using Pizzario.Services.ProductApi.Models.Dto;

namespace Pizzario.Services.ProductApi.Controllers
{
    [Route("api/ProductAPI")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;

        public ProductAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto GetProductList()
        {
            try
            {
                IEnumerable<Product> productList = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(productList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto GetProductById(int id)
        {
            try
            {
                Product product = _db.Products.First(x => x.ProductId == id);
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetProductByCategory/{category}")]
        public ResponseDto GetProductByCategory(string category)
        {
            try
            {
                IEnumerable<Product> productList = _db.Products.ToList().Where(x => x.CategoryName.ToLower() == category.ToLower());
                //Product product = _db.Products.First(x => x.CategoryName.ToLower() == category.ToLower());
                _response.Result = _mapper.Map< IEnumerable<ProductDto>>(productList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public ResponseDto AddProduct([FromBody] ProductDto product)
        {
            var _response = new ResponseDto();

            try
            {
                Product obj = _mapper.Map<Product>(product);
                _db.Add(obj);
                _db.SaveChanges();
                _response.IsSuccess = true;
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPut]
        public ResponseDto UpdateProduct([FromBody] ProductDto product)
        {
            var _response = new ResponseDto();

            try
            {
                Product obj = _mapper.Map<Product>(product);
                _db.Update(obj);
                _db.SaveChanges();
                _response.IsSuccess = true;
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]

        public ResponseDto DeleteProduct(int id)
        {
            try
            {
                Product product = _db.Products.First(x => x.ProductId == id);
                _db.Products.Remove(product);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
