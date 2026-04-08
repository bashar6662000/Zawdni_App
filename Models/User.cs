using Zawdni.Models;

namespace Zawdni.api.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public  string Email { get; set; } = string.Empty;
        public required string Password { get; set; }
        public  string PhoneNumber { get; set; } =string.Empty;
        public string Role { get; set; } = "Doctor";
        public string ClinicName { get; set; } = "N/A";
        public string ClinicAddress { get; set; } = "N/A";
        public List<Order> orders { get; set; }
    }
}
