using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pizzario.Web.Models;
using Pizzario.Web.Service.IService;

namespace Pizzario.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<Coupons>? couponsList = new();
            ResponseDto? response = await _couponService.GetAllCouponsAsync();
            if (response != null && response.IsSuccess)
            {
                couponsList = JsonConvert.DeserializeObject<List<Coupons>>(Convert.ToString(response.Result));
            }
            return View(couponsList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(Coupons coupons)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ResponseDto response = await _couponService.CreateCouponAsync(coupons);
                    if (response.IsSuccess)
                    {
                        TempData["success"] = "Copon created successfully!";
                        return RedirectToAction("CouponIndex");
                    }
                    else
                    {
                        TempData["error"] = response?.Message;
                        ModelState.AddModelError("", "Failed to create coupon. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred: {ex.Message}");
                }
            }
            return View("CouponIndex", await _couponService.GetAllCouponsAsync());
        }

        //[HttpDelete]
        public async Task<IActionResult> DeleteCoupon(int couponId)
        {
            ResponseDto? response = await _couponService.DeleteCouponAsync(couponId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Copon deleted successfully!";
                return RedirectToAction("CouponIndex");

            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }
    }
}
