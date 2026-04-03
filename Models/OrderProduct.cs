using Zawdni.api.Models;

namespace Zawdni.Models
{
    public class OrderProduct
    {
        public int OrderId { get; set; }
        public Order order { get; set; }

        public int ProductId { get; set; }
        public Product product { get; set; }

        public int Quantity { get; set; } // الكمية اللي انطلبت
        public decimal UnitPrice { get; set; } // السعر وقت الطلب (عشان لو تغير سعر المنتج بالمستودع ما تخرب فواتيرك القديمة)
    }
}
