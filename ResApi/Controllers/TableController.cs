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
    public class TableController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITable _table;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public TableController(IUnitOfWork unitOfWork, ITable table)
        {
            _unitOfWork = unitOfWork;
            _table = table;
        }
        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<Table>> GetTable(int tableId, CancellationToken cancellationToken)
        {
            return Ok(await _table.Get(tableId, cancellationToken));
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<Table>>> GetAllTables(CancellationToken cancellationToken)
        {
            return Ok(await _table.GetAll(cancellationToken));
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Table>> CreateTable([FromBody] Table entity, CancellationToken cancellationToken)
        {
            _table.Add(entity);
            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<Table>> UpdateTable([FromBody] Table entity, CancellationToken cancellationToken)
        {
            _table.Update(entity);
            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult> DeleteTable([FromBody] int tableId, CancellationToken cancellationToken)
        {
            var entity = await _table.Get(tableId, cancellationToken);

            if (entity == null)
                return BadRequest("No entity was found with the provided ID.");


            _table.Delete(entity);

            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

    }
}

