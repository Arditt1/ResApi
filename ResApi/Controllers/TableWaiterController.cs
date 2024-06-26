using Microsoft.AspNetCore.Mvc;
using NLog;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces;
using ResApi.DTO.Tables;
using ResApi.DTO.TableWaiter;
using ResApi.Models;
using System;
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
            try
            {
                var response = await _tableWaiter.Get(tableWaiterId, cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register user"

                };
                return BadRequest(errRet);
            }
        }
        [HttpGet]
        [Route("getMyTables")]
        public async Task<List<TableDTO>> MyTables(int waiterId)
        {
            return await _tableWaiter.MyTables(waiterId);
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<TableWaiter>>> GetAllTableWaiters(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _tableWaiter.GetAll(cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register user"

                };
                return BadRequest(errRet);
            }
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
        [Route("register")]
        public async Task<ActionResult<DataResponse<string>>> Register(TableWaiterDTO entity)
        {
            try
            {
                var addingTableWaiter = await _tableWaiter.Register(entity);
                return Ok(addingTableWaiter);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register user"

                };
                return BadRequest(errRet);
            }
        }
        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<TableWaiter>> UpdateTableWaiter([FromBody] TableWaiterDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                var response = _tableWaiter.UpdateTableWaiter(entity);
                await _unitOfWork.Save(cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register user"

                };
                return BadRequest(errRet);
            }
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

