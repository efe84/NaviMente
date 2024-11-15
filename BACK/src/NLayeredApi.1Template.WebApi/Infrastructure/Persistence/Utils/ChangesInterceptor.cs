using Microsoft.EntityFrameworkCore.Diagnostics;

namespace NaviMente.WebApi.Infrastructure.Persistence.Utils
{
    public class ChangesInterceptor : SaveChangesInterceptor
    {
        readonly IServiceProvider _serviceProvider;

        public ChangesInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        //public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        //{
        //    eventData.Context?.ChangeTracker?.Entries().ToList().ForEach(entry =>
        //    {
        //        if (entry.Entity.GetType().IsSubclassOf(typeof(AggregateRoot)))
        //        {
        //            DispatchEvents((AggregateRoot)entry.Entity);
        //        }
        //    });
        //    return base.SavedChanges(eventData, result);
        //}

        //private void DispatchEvents(AggregateRoot aggregateRoot)
        //{
        //    aggregateRoot.DomainEvents.ToList().ForEach(domainEvent => {
        //        DomainEvents.Dispatch(domainEvent, _serviceProvider);
        //    });
        //    aggregateRoot.ClearEvents();
        //}
    }
}
