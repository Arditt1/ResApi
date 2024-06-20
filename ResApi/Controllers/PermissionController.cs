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
            return Ok(await _permission.Get(permissionId, cancellationToken));
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<Permission>>> GetAllPermissions(CancellationToken cancellationToken)
        {
            return Ok(await _permission.GetAll(cancellationToken));
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Permission>> CreatePermission([FromBody] Permission entity, CancellationToken cancellationToken)
        {
            _permission.Add(entity);
            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<Permission>> UpdatePermission([FromBody] Permission entity, CancellationToken cancellationToken)
        {
            _permission.Update(entity);
            await _unitOfWork.Save(cancellationToken);

            return Ok();
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

