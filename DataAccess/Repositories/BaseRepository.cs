using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class   {
        protected WebApiDbContext _context;
        public BaseRepository(WebApiDbContext dbContext) {
            _context = dbContext;
        }
        public T Add(T entity) {
            _context.Add(entity);
            return entity;
        }
        public T Edit(T entity) {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        protected IQueryable<T> GetQueryable() {
            return GetQueryable(x => true);
        }

        protected IQueryable<T> GetQueryable(Expression<Func<T, bool>> match) {
            return _context.Set<T>().Where(match);
        }

        public List<T> Get() {
            return Get(x => true, x => x);
        }

        public List<T> Get(Expression<Func<T, bool>> match)
        {
            return Get(match, x => x);
        }
        public List<R> Get<R>(Expression<Func<T, bool>> match, Expression<Func<T, R>> select) {

            var q = this.GetQueryable(match);
            return q.Select(select).ToList();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> match)
        {
            return this.GetQueryable(match).SingleOrDefault();
        }

        public T Single(Expression<Func<T, bool>> match)
        {
            return this.GetQueryable(match).Single();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> match)
        {
            return this.GetQueryable(match).FirstOrDefault();
        }

        public T First(Expression<Func<T, bool>> match)
        {
            return this.GetQueryable(match).First();
        }

        public T LastOrDefault(Expression<Func<T, bool>> match)
        {
            return this.GetQueryable(match).LastOrDefault();
        }

        public T Last(Expression<Func<T, bool>> match)
        {
            return GetQueryable(match).Last();
        }
        
        public T Find(int id) {
            return _context.Find<T>(id);
        }

        public Task<T> FindAsync(int id) {
            return _context.FindAsync<T>(id);
        }

        public void Save() {
            _context.SaveChanges();
        }

        public void Remove(T entity) {
            _context.Remove(entity);

        }

        public void Remove(int id) {
            _context.Remove(Find(id));
        }

        public Task SaveAsync() {
            return _context.SaveChangesAsync();
        }

        public bool Any(Expression<Func<T, bool>> match)
        {
            return this.GetQueryable(match).Any();
        }
    }
}