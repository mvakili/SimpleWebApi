using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        T Edit(T entity);

        List<T> Get();
        List<R> Get<R>(Expression<Func<T, bool>> match, Expression<Func<T, R>> select);

        T SingleOrDefault(Expression<Func<T, bool>> match);

        T Single(Expression<Func<T, bool>> match);

        T FirstOrDefault(Expression<Func<T, bool>> match);

        T First(Expression<Func<T, bool>> match);

        T LastOrDefault(Expression<Func<T, bool>> match);

        T Last(Expression<Func<T, bool>> match);

        T Find(int id);

        bool Any(Expression<Func<T, bool>> match);

        Task<T> FindAsync(int id);

        void Save();

        void Remove(T entity);

        void Remove(int id);

        Task SaveAsync();
    }
}
