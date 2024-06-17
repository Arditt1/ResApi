using System;
using System.Threading;
using System.Threading.Tasks;

namespace ResApi.DTA.Intefaces
{
	public interface IUnitOfWork
	{
        Task Save(CancellationToken cancellationToken);
    }
}

