using Zawdni.api.Models;

namespace Zawdni.Models
{
    public class Order
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required bool State {  get; set; } 
        public int UserID { get; set; }

        public User user { get; set; }
        public List<OrderProduct> orderProducts {  get; set; }
    }
}
