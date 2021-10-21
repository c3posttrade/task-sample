using System.Linq;

namespace Shared.Core.Persistence
{
    public interface IDataTable<T> : IQueryable<T> where T : class
    {
        void Add(T item);
        void Remove(T item);
        IQueryable<T> AsNoTracking();
    }
}
