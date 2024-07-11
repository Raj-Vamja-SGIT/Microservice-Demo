using Pizzario.Web.Models;

namespace Pizzario.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto> GetCouponAsync(int couponCode);       
        Task<ResponseDto> GetAllCouponsAsync();       
        Task<ResponseDto> GetCouponByIdAsync(int couponId);       
        Task<ResponseDto> CreateCouponAsync(Coupons coupons);       
        Task<ResponseDto> UpdateCouponAsync(Coupons coupons);       
        Task<ResponseDto> DeleteCouponAsync(int couponId);       
    }
}
