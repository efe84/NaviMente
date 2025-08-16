using NaviMente.WebApi.Dto.Enums;

namespace NaviMente.WebApi.Dto.User
{
    public class UserRegisterDTO
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string MainPhone { get; set; }
        public string? SerialNumber { get; set; }
        public string? DeviceName { get; set; }
    }
}
