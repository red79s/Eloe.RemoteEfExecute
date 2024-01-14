using System.Linq.Expressions;

namespace Eloe.RemoteEfExecute
{
    public interface IRemoteDbSet<T> : IRemoteDbSetBase
    {
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
        Task<T?> FirstOrDefaultAsync();
        IRemoteDbSet<T> Include(string navigationPropertyPath);
        Task<IList<T>> ToListAsync();
        IRemoteDbSet<T> Where(Expression<Func<T, bool>> expression);
        void Add(T item);
        void Update(T item);
        void Delete(T item);
    }
}