namespace NaviMente.WebApi.Dto.User
{
    public class UserEditDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public long? DeviceId { get; set; }
    }
}
