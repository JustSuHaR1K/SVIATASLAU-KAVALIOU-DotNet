using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventus.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task Create(TEntity item);

        Task AddRange(IEnumerable<TEntity> list);

        Task<TEntity> FindById(int id);

        Task<IEnumerable<TEntity>> Get();

        Task Remove(TEntity item);

        Task Update(TEntity item);
    }
}