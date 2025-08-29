using NaviMente.WebApi.Domain.Shared.Entities;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories
{
    public interface IUserQueryRepository
    {
        User? GetByUsername(string username);
        User? GetByUserId(long userId);
        void InsertUser(User user);
        void UpdateEmail(string username, string newEmail);
        void UpdateMainPhone(string username, string newMainPhone);
        void AddPhone(string username, string newPhoneNumber);
        void RemovePhone(string username, string phoneNumber);
        void UnlinkTelegram(string userId);
    }
}
