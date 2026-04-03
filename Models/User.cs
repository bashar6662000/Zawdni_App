using Zawdni.Models;

namespace Zawdni.api.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string PhoneNumber { get; set; } 
        public required string Role { get; set; }
        public required string ClinicName { get; set; } 
        public required string ClinicAddress {  get; set; } 
        public List<Order> orders { get; set; }
    }
}
