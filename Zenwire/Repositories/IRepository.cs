using System.Linq;

namespace Zenwire.Repositories
{
    public interface IRepository<T>  where T : class 
    {
        IQueryable<T> Get { get; }
        T Find(object[] keyValues);
        T Find(int id);
        void Add(T entity);
        void Update(T entity);
        void AddOrUpdate(T entity);
        void Remove(object[] keyValues);
        void Remove(T entity);
        
    }
}