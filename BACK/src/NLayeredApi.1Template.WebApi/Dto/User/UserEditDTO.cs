namespace NaviMente.WebApi.Dto.User
{
    public class UserEditDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? MainPhone { get; set; }
        public List<string>? OtherPhones { get; set; }
        public string? SerialNumber { get; set; }
    }
}
