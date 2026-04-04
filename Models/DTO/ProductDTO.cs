namespace Zawdni.Models.DTO
{
    public class ProductDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public int Quntity { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0;
    }
}
