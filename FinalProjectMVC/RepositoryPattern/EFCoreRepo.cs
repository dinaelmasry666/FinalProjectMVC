﻿using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public abstract class EFCoreRepo<T> : IRepository<T>
        where T : class
    {
        protected readonly DbContext _context;

        public EFCoreRepo(DbContext context) => _context = context;

        public virtual void Insert(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public virtual void Update<PType>(PType id, T entity)
        {
            var existingEntity = _context.Set<T>().Find(id);

            if (existingEntity is not null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
        }

        public virtual void Delete<PType>(PType id)
        {
            var existingEntity = _context.Set<T>().Find(id);

            if (existingEntity is not null)
            {
                _context.Set<T>().Remove(existingEntity);
                _context.SaveChanges();
            }
        }

        public virtual T? GetDetails<PType>(PType id) => _context.Set<T>().Find(id);

        public virtual List<T> GetAll() => _context.Set<T>().ToList();

        public virtual List<T> Filter(Func<T, bool> filterPredicate)
        {
            return _context.Set<T>().Where(filterPredicate).ToList();
        }

        public virtual Task<List<T>> GetAllAsync() => _context.Set<T>().ToListAsync();

        public virtual async Task<T?> GetDetailsAsync<PType>(PType id) => await _context.Set<T>().FindAsync(id);

        public async virtual Task InsertAsync(T Entity)
        {
            await _context.Set<T>().AddAsync(Entity);
            await _context.SaveChangesAsync();
        }

        public async virtual Task UpdateAsync<PType>(PType id, T Entity)
        {
            var existingEntity = await _context.Set<T>().FindAsync(id);

            if (existingEntity is not null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(Entity);
                await _context.SaveChangesAsync();
            }
        }

        public async virtual Task DeleteAsync<PType>(PType id)
        {
            var existingEntity = await _context.Set<T>().FindAsync(id);

            if (existingEntity is not null)
            {
                _context.Set<T>().Remove(existingEntity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual Task<List<T>> FilterAsync(Func<T, bool> filterPredicate)
        {
            return Task.FromResult(_context.Set<T>().Where(filterPredicate).ToList());
        }
    }
}