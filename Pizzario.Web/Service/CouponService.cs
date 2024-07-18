using Pizzario.Web.Models;
using Pizzario.Web.Service.IService;
using Pizzario.Web.Utility;

namespace Pizzario.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> CreateCouponAsync(Coupons coupons)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = coupons,
                Url = StaticDetails.CouponApiBase + "/api/CouponsAPI"
            });
        }

        public async Task<ResponseDto> DeleteCouponAsync(int couponId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.CouponApiBase + "/api/CouponsAPI/" + couponId
            });
        }

        public async Task<ResponseDto> GetAllCouponsAsync(string baseUrl)
        {
            //return await _baseService.SendAsync(new RequestDto()
            //{
            //    ApiType = StaticDetails.ApiType.GET,
            //    Url = StaticDetails.CouponApiBase + "/api/CouponsAPI"
            //});

            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = $"{baseUrl}/coupons"
            });
        }

        public async Task<ResponseDto> GetCouponAsync(int couponCode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.CouponApiBase + "/api/CouponsAPI/GetCouponByCode/" + couponCode
            });
        }

        public async Task<ResponseDto> GetCouponByIdAsync(int couponId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.CouponApiBase + "/api/CouponsAPI/" + couponId
            });
        }

        public async Task<ResponseDto> UpdateCouponAsync(Coupons coupons)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = coupons,
                Url = StaticDetails.CouponApiBase + "/api/CouponsAPI"
            });
        }
    }
}
