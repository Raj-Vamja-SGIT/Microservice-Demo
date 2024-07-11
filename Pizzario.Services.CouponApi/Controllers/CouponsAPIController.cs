using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pizzario.Services.CouponApi.Data;
using Pizzario.Services.CouponApi.Models;
using Pizzario.Services.CouponApi.Models.Dto;

namespace Pizzario.Services.CouponApi.Controllers
{
    [Route("api/CouponsAPI")]
    [ApiController]
    public class CouponsAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;

        public CouponsAPIController(AppDbContext db)
        {
            _db = db;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto GetCouponsList()
        {
            try
            {
                IEnumerable<Coupons> couponsList = _db.Coupons.ToList();
                _response.Result = couponsList;
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
        public ResponseDto GetCouponById(int id)
        {
            try
            {
                Coupons coupon = _db.Coupons.First(x => x.CouponId == id);
                _response.Result = coupon;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetCouponByCode/{code}")]
        public ResponseDto GetCouponByCode(string code)
        {
            try
            {
                Coupons couponCode = _db.Coupons.First(x => x.CouponCode.ToLower() == code.ToLower());
                _response.Result = couponCode;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        
        [HttpPost]
        public ResponseDto AddCoupon([FromBody] Coupons coupons)
        {
            var _response = new ResponseDto();

            try
            {
                _db.Add(coupons);
                _db.SaveChanges();
                _response.IsSuccess = true;
                _response.Result = coupons; 
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        
        
        [HttpPut]
        public ResponseDto UpdateCoupon([FromBody] Coupons coupons)
        {
            var _response = new ResponseDto();

            try
            {
                _db.Update(coupons);
                _db.SaveChanges();
                _response.IsSuccess = true;
                _response.Result = coupons; 
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

        public ResponseDto DeleteCoupon(int id)
        {
            try
            {
                Coupons couponId = _db.Coupons.First(x => x.CouponId == id);
                _db.Coupons.Remove(couponId);
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
