using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pizzario.Services.ShoppingCartApi.Data;
using Pizzario.Services.ShoppingCartApi.Models;
using Pizzario.Services.ShoppingCartApi.Models.Dto;
using System.Reflection.PortableExecutable;

namespace Pizzario.Services.ShoppingCartApi.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;

        public ShoppingCartAPIController(AppDbContext db)
        {
            _db = db;
            _response = new ResponseDto();
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
