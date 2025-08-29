namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories
{
    public interface ICounterQueryRepository
    {
        long GetNextSequenceValue(string sequenceName);
    }
}
