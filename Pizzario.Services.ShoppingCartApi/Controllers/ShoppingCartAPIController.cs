using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzario.Services.ShoppingCartApi.Data;
using Pizzario.Services.ShoppingCartApi.Models;
using Pizzario.Services.ShoppingCartApi.Models.Dto;
using Pizzario.Services.ShoppingCartApi.Service.IService;
using System.Reflection.PortableExecutable;

namespace Pizzario.Services.ShoppingCartApi.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IProductService _productService;

        public ShoppingCartAPIController(AppDbContext db, IProductService productService)
        {
            _db = db;
            _response = new ResponseDto();
            _productService = productService;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                var cartHeaderFromDb = _db.CartHeaders.First(u => u.UserId == userId);

                CartDto cart = new CartDto()
                {
                    CartHeader = new CartHeaders
                    {
                        CartHeaderId = cartHeaderFromDb.CartHeaderId,
                        UserId = cartHeaderFromDb.UserId,
                        CouponCode = cartHeaderFromDb.CouponCode,
                        Discount = cartHeaderFromDb.Discount,
                        CartTotal = cartHeaderFromDb.CartTotal,
                    }
                };

                var cartDetailsFromDb = _db.CartDetails
            .Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId)
            .ToList();

                cart.CartDetails = cartDetailsFromDb.Select(detail => new CartDetails
                {
                    CartDetailsId = detail.CartDetailsId,
                    CartHeaderId = detail.CartHeaderId,
                    ProductId = detail.ProductId,
                    Count = detail.Count,
                }).ToList();

                IEnumerable<Product> productDtos = await _productService.GetProducts();

                foreach (var item in cart.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(u => u.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                ////apply coupon if any
                //if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                //{
                //    CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                //    if (coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                //    {
                //        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                //        cart.CartHeader.Discount = coupon.DiscountAmount;
                //    }
                //}

                _response.Result = cart;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost("CartInsertUpdate")]
        public async Task<ResponseDto> CartInsertUpdate(CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    CartHeaders cartHeader = new CartHeaders
                    {
                        UserId = cartDto.CartHeader.UserId,
                        CouponCode = cartDto.CartHeader.CouponCode,
                        CartHeaderId = cartDto.CartHeader.CartHeaderId,
                        Discount = cartDto.CartHeader.Discount,
                        CartTotal = cartDto.CartHeader.CartTotal,
                    };
                    _db.CartHeaders.Add(cartHeader);
                    await _db.SaveChangesAsync();

                    CartDetails cartDetails = new CartDetails
                    {
                        CartHeaderId = cartHeader.CartHeaderId,
                        ProductId = cartDto.CartDetails.First().ProductId,
                        Count = cartDto.CartDetails.First().Count,
                        CartDetailsId = cartDto.CartDetails.First().CartDetailsId,
                        CartHeader = cartDto.CartDetails.First().CartHeader,
                        Product = cartDto.CartDetails.First().Product,
                    };
                    _db.CartDetails.Add(cartDetails);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cartDto.CartDetails.First().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        //create cartdetails
                        CartDetails cartDetails = new CartDetails
                        {
                            CartHeaderId = cartHeaderFromDb.CartHeaderId,
                            ProductId = cartDto.CartDetails.First().ProductId,
                            Count = cartDto.CartDetails.First().Count,
                            // add other properties as necessary
                        };
                        _db.CartDetails.Add(cartDetails);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        //update count in cart details
                        cartDetailsFromDb.Count += cartDto.CartDetails.First().Count;
                        _db.CartDetails.Update(cartDetailsFromDb);
                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }


        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = _db.CartDetails
                   .First(u => u.CartDetailsId == cartDetailsId);

                int totalCountofCartItem = _db.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();
                _db.CartDetails.Remove(cartDetails);
                if (totalCountofCartItem == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders
                       .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync();

                _response.Result = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.Message.ToString();
                _response.IsSuccess = false;
            }
            return _response;
        }

    }
}
