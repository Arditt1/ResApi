using Microsoft.AspNetCore.Mvc;
using NLog;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces;
using ResApi.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPermission _permission;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public PermissionController(IUnitOfWork unitOfWork, IPermission permission)
        {
            _unitOfWork = unitOfWork;
            _permission = permission;
        }
        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<Permission>> GetPermission(int permissionId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _permission.Get(permissionId, cancellationToken);
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
            //return Ok(await _permission.Get(permissionId, cancellationToken));
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<Permission>>> GetAllPermissions(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _permission.GetAll(cancellationToken);
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
        public async Task<ActionResult<string>> CreatePermission([FromBody] PermissionDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _permission.CreatePermission(entity);
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
        [Route("update")]
        public async Task<ActionResult<string>> UpdatePermission([FromBody] PermissionDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _permission.UpdatePermission(entity);
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
        public async Task<ActionResult> DeletePermission([FromBody] int permissionId, CancellationToken cancellationToken)
        {
            var entity = await _permission.Get(permissionId, cancellationToken);

            if (entity == null)
                return BadRequest("No entity was found with the provided ID.");


            _permission.Delete(entity);

            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

    }
}

