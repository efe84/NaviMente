
using NaviMente.WebApi.Dto.Enums;

namespace NaviMente.WebApi.Dto.Login
{
    public class UserRegisterDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? MainPhone { get; set; }
        public string? SerialNumber { get; set; }
    }
}
