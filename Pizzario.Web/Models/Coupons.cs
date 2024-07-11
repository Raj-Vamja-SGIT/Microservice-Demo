using System.ComponentModel.DataAnnotations;

namespace Pizzario.Web.Models
{
    public class Coupons
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
