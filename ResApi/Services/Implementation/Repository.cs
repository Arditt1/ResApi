using ResApi.Services.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Serilog;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;
using ResApi.Models;

namespace ResApi.Services.Implementation
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly IDbContextFactory _dbContextFactory;
        protected ILogger<TEntity> Logger;

        public Repository(IDbContextFactory dbContextFactory, ILogger<TEntity> logger)
        {
            _dbContextFactory = dbContextFactory;
            Logger = logger;
        }

        protected RestApiDbContext DbContext => _dbContextFactory?.DbContext;
        /// <summary>
        /// Get Entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TEntity> GetEntity(object id)
        {
            var entity = await DbContext.FindAsync<TEntity>(id);
            return entity;
        }
        /// <summary>
        /// Add Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> AddEntity(TEntity entity)
        {
            try
            {
                var result = await DbContext.AddAsync<TEntity>(entity);
                await DbContext.SaveChangesAsync();
                return result.Entity;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Unhandled Exception");
                throw;
            }
        }
        /// <summary>
        /// Update Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> UpdateEntity(TEntity entity)
        {
            DbContext.Update<TEntity>(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }
        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteEntity(object id)
        {
            var entity = await DbContext.FindAsync<TEntity>(id);
            if (entity != null)
            {
                DbContext.Remove<TEntity>(entity);
                await DbContext.SaveChangesAsync();
            }
            return true;
        }
    }
}
