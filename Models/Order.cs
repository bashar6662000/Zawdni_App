using Zawdni.api.Models;

namespace Zawdni.Models
{
    public class Order
    {
        public int Id { get; set; }
        public required bool State {  get; set; } = false;
        public DateTime OrderDate { get; set; }= DateTime.Now;
        public DateTime? DeliveryDate { get; set; }
        public decimal Total { get; set; }
        public int UserID { get; set; }

        public User user { get; set; }
        public List<OrderProduct> orderProducts {  get; set; }
    }
}
