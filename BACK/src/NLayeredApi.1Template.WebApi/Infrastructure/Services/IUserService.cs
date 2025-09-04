using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.User;

namespace NaviMente.WebApi.Infrastructure.Services
{
    public interface IUserService
    {
        long CreateUser(UserRegisterDTO userRegister);
        bool ValidatePassword(string userName, string password);
        User GetUser(string userName);
        User? EditEmail(string username, string newEmail);
        User? EditMainPhone(string username, string newMainPhone);
        string GenerateLinkCode(string userId);
        void UnlinkTelegram(string userId);
    }
}
