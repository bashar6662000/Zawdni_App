namespace Zawdni.Models.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required bool State { get; set; }
    }
}
