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
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRole _role;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public RoleController(IUnitOfWork unitOfWork, IRole role)
        {
            _unitOfWork = unitOfWork;
            _role = role;
        }
        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<Role>> GetRole(int roleId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _role.Get(roleId, cancellationToken);
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
        public async Task<ActionResult<List<Role>>> GetAllRoles(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _role.GetAll(cancellationToken);
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
        public async Task<ActionResult<Role>> CreateRole([FromBody] RoleDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _role.CreateRole(entity);
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
        public async Task<ActionResult<Role>> UpdateRole([FromBody] RoleDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _role.UpdateRole(entity);
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
        public async Task<ActionResult> DeleteRole([FromBody] int roleId, CancellationToken cancellationToken)
        {
            var entity = await _role.Get(roleId, cancellationToken);

            if (entity == null)
                return BadRequest("No entity was found with the provided ID.");


            _role.Delete(entity);

            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

        [HttpGet]
        [Route("getAllMenuItemsByCategory")]
        public async Task<List<RoleDTO>> GetAllMenuItemsByCategory(string rolename)
        {
            var entity = await _role.GetEmployeesByRole(rolename);
            return entity;
        }

    }
}

