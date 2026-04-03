namespace Zawdni.Models.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Role { get; set; }
        public required string ClinicName { get; set; }
        public required string ClinicAddress { get; set; }

    }
}
