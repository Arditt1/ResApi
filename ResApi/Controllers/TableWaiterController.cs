using Microsoft.AspNetCore.Mvc;
using NLog;
using ResApi.DTA.Intefaces;
using ResApi.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableWaiterController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITableWaiter _tableWaiter;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public TableWaiterController(IUnitOfWork unitOfWork, ITableWaiter tableWaiter)
        {
            _unitOfWork = unitOfWork;
            _tableWaiter = tableWaiter;
        }
        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<TableWaiter>> GetTableWaiter(int tableWaiterId, CancellationToken cancellationToken)
        {
            return Ok(await _tableWaiter.Get(tableWaiterId, cancellationToken));
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<TableWaiter>>> GetAllTableWaiters(CancellationToken cancellationToken)
        {
            return Ok(await _tableWaiter.GetAll(cancellationToken));
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<TableWaiter>> CreateTableWaiter([FromBody] TableWaiter entity, CancellationToken cancellationToken)
        {
            _tableWaiter.Add(entity);
            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<TableWaiter>> UpdateTableWaiter([FromBody] TableWaiter entity, CancellationToken cancellationToken)
        {
            _tableWaiter.Update(entity);
            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult> DeleteTableWaiter([FromBody] int tableWaiterId, CancellationToken cancellationToken)
        {
            var entity = await _tableWaiter.Get(tableWaiterId, cancellationToken);

            if (entity == null)
                return BadRequest("No entity was found with the provided ID.");


            _tableWaiter.Delete(entity);

            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

    }
}

