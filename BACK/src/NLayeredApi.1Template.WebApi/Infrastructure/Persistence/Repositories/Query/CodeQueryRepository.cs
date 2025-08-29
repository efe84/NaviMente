using MongoDB.Driver;
using NaviMente.WebApi.Domain.Shared.Entities;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories.Query
{
    public class CodeQueryRepository: ICodeQueryRepository
    {
        private readonly IMongoCollection<TelegramLinkCode> _codesCollection;

        public CodeQueryRepository(IApplicationContext dbContext)
        {
            _codesCollection = dbContext.Codes;
        }

        public void DeleteOldcodes(long userId)
        {
            _codesCollection.DeleteMany(c => c.UserId == userId);
        }

        public void InsertCode(TelegramLinkCode codeEntry)
        {
            _codesCollection.InsertOne(codeEntry);
        }
    }
}
