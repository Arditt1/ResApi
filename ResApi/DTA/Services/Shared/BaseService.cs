using Microsoft.EntityFrameworkCore;
using ResApi.Models.Shared;
using ResApi.Models;
using System.Threading.Tasks;
using ResApi.DTA.Intefaces.Shared;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace ResApi.DTA.Services.Shared
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        protected readonly DataContext Context;

        public BaseService(DataContext context)
        {
            Context = context;
        }

        public void Add(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public Task<T?> Get(int id, CancellationToken cancellationToken)
        {
            return Context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public void Delete(T entity)
        {
            entity.Deleted = true;
            Context.Update(entity);
            Context.SaveChanges();
        }

        public void Update(T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }

        public Task<List<T>> GetAll(CancellationToken cancellationToken)
        {
            return Context.Set<T>().Where(x=>x.Deleted == false).ToListAsync(cancellationToken);
        }

        public IQueryable<T> GetAllQueryable()
        {
            return Context.Set<T>().AsNoTracking();
        }
    }
}

