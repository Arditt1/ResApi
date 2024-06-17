using ResApi.DTA.Intefaces;
using ResApi.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ResApi.DTA.Services.Shared
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }
        public Task Save(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}

