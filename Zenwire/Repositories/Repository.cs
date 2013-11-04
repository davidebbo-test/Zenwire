using System;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Zenwire.Extensions;

namespace Zenwire.Repositories
{
    [ExcludeFromCodeCoverage]
    public class Repository<T> : IRepository<T> where T : class
    {
        readonly ZenwireContext _context;

        public Repository(ZenwireContext context) 
        {
            _context = context;
        }
 
        public IQueryable<T> Get
        {
            get { return _context.Set<T>(); }
        }
 
        public IQueryable<T> GetIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
 
        public T Find(object[] keyValues)
        {
            return _context.Set<T>().Find(keyValues);
        }

        public T Find(int id)
        {
            return _context.Set<T>().Find(id);
        }


        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
            _context.Entry(entity).GetDatabaseValues();

            //return entity.ID;
        }

        
        public T FindBy(T entity)
        {
            return _context.Set<T>().Find(entity);
        }
 
        public void Update(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
                entry = _context.Entry(entity);
            }
            entry.State = EntityState.Modified;
            _context.SaveChanges();
        }
 
        public void AddOrUpdate(T entity)
        {
            //uses DbContextExtensions to check value of primary key
            _context.AddOrUpdate(entity);
            _context.SaveChanges();
        }

        public void Remove(object[] keyValues)
        {
            //uses DbContextExtensions to attach a stub (or the actual entity if loaded)
            var stub = _context.Load<T>(keyValues);
            _context.Set<T>().Remove(stub);
            _context.SaveChanges();
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public void Dispose(bool disposing)
        {

        }
    }
}