namespace Zawdni.Models.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public required bool State { get; set; } = false;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime? DeliveryDate { get; set; }
        public decimal Total { get; set; }

        public int UserID { get; set; }
        public List<OrderProductDTO> Products { get; set; } = new();
    }
}
