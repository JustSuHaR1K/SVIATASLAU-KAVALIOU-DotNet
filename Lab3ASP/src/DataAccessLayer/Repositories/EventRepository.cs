using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eventus.DAL.Interfaces;

namespace EventusDAL.Repositories
{
    public class EventRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly EventContext _context;

        private readonly DbSet<TEntity> _dbSet;

        public EventRepository(EventContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Get()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> FindById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task Create(TEntity item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Remove(TEntity item)
        {
            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task AddRange(IEnumerable<TEntity> list)
        {
            await _dbSet.AddRangeAsync(list);
            await _context.SaveChangesAsync();
        }

        
    }
}