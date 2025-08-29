using NaviMente.WebApi.Domain.Shared.Entities;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories
{
    public interface ICodeQueryRepository
    {
        void DeleteOldcodes(long userId);
        void InsertCode(TelegramLinkCode codeEntry);
    }
}
