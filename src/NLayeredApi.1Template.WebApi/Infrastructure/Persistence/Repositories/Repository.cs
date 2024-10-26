using Microsoft.EntityFrameworkCore;

namespace NLayeredApi._1Template.WebApi.Infrastructure.Persistence
{
    //public abstract class Repository<T, Key> : IRepository<T, Key> where T : Entity
    //{
    //    protected readonly ApplicationContext _context;

    //    protected readonly DbSet<T> dbSet;

    //    protected Repository(ApplicationContext context)
    //    {
    //        _context = context;
    //        dbSet = _context.Set<T>();
    //    }

    //    public virtual List<T> GetAll()
    //    {
    //        return dbSet.OrderBy(e => e.Id).ToList();
    //    }

    //    public virtual T? Find(Key key)
    //    {
    //        return dbSet.Find(key);
    //    }

    //    public void Create(T entity)
    //    {
    //        dbSet.Add(entity);
    //    }

    //    public void Delete(T entity)
    //    {
    //        dbSet.Remove(entity);
    //    }

        
    //}

    //public abstract class Repository<T> : Repository<T, long> where T : Entity
    //{
    //    protected Repository(ApplicationContext context) : base(context)
    //    {
    //    }
    //}
}
