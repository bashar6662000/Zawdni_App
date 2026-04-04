using Zawdni.Models;

namespace Zawdni.api.Models
{
    public class Product
    {
        public  int Id { get; set; } 
        public required string Name { get; set; }
        public int Quntity { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;

        public List<OrderProduct> orderProducts { get; set; }

    }
}
