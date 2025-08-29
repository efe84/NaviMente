using MongoDB.Driver;
using NaviMente.WebApi.Domain.Shared.Entities;
using System.Diagnostics.CodeAnalysis;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories.Query
{
    public class CounterQueryRepository: ICounterQueryRepository
    {
        private readonly IMongoCollection<Counter> _countersCollection;

        public CounterQueryRepository(IApplicationContext dbContext)
        {
            _countersCollection = dbContext.Counters;
        }

        public long GetNextSequenceValue(string sequenceName)
        {
            var filter = Builders<Counter>.Filter.Eq(c => c.Id, sequenceName);
            var update = Builders<Counter>.Update.Inc(c => c.SequenceValue, 1);

            var options = new FindOneAndUpdateOptions<Counter>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var updatedCounter = _countersCollection.FindOneAndUpdate(filter, update, options);
            return updatedCounter.SequenceValue;
        }
    }
}
