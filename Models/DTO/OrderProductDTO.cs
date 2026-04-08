namespace Zawdni.Models.DTO
{
    public class OrderProductDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } // الكمية اللي انطلبت
        public decimal UnitPrice { get; set; } // السعر وقت الطلب (عشان لو تغير سعر المنتج بالمستودع ما تخرب فواتيرك القديمة)

    }
}
