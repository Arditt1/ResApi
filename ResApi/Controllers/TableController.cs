﻿using Microsoft.AspNetCore.Mvc;
using NLog;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces;
using ResApi.DTO.Tables;
using ResApi.Models;
using System;
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
            try
            {
                var response = await _table.Get(tableId, cancellationToken);
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
        [Route("getAll")]
        public async Task<ActionResult<List<Table>>> GetAllTables(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _table.GetAll(cancellationToken);
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
        public async Task<ActionResult<Table>> CreateTable([FromBody] Table entity, CancellationToken cancellationToken)
        {
            try
            {
                var response = "";//await _role.CreateRole(entity);
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
        [Route("register")]
        public async Task<DataResponse<string>> Register(TableDTO entity)
        {
            try
            {
                var addingTable = await _table.Register(entity);
                return addingTable;
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<Table>> UpdateTable([FromBody] Table entity, CancellationToken cancellationToken)
        {
            _table.Update(entity);
            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }
        [HttpGet]
        [Route("getFreeTables")]
        public async Task<List<TableDTO>> FreeTables()
        {
            return await _table.FreeTables();
        }
        [HttpDelete]
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

