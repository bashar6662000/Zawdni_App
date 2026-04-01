namespace Zawdni.api.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public required string Role { get; set; }
        public string ClinicName { get; set; } = string.Empty;
        public string ClinicAddress {  get; set; } = string.Empty;
    }
}
