using ResApi.Models.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ResApi.DTA.Intefaces.Shared
{
    public interface IBaseService<T> where T : BaseEntity
    {
        void Add(T entity);
        Task<T?> Get(int id, CancellationToken cancellationToken);
        void Delete(T entity);
        void Update(T entity);
        Task<List<T>> GetAll(CancellationToken cancellationToken);
        IQueryable<T> GetAllQueryable();
    }
}

