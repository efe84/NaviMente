using MongoDB.Bson;
using MongoDB.Driver;

namespace NaviMente.WebApi.Infrastructure.Services
{
    public class CounterService
    {
        private readonly IMongoCollection<BsonDocument> _countersCollection;

        public CounterService(IMongoDatabase database)
        {
            _countersCollection = database.GetCollection<BsonDocument>("counters");
        }

        public async Task<int> GetNextSequenceValueAsync(string counterName)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", counterName);
            var update = Builders<BsonDocument>.Update.Inc("sequenceValue", 1);
            var options = new FindOneAndUpdateOptions<BsonDocument>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var result = await _countersCollection.FindOneAndUpdateAsync(filter, update, options);
            return result["sequence_value"].AsInt32;
        }
    }
}
