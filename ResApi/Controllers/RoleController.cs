using Microsoft.AspNetCore.Mvc;
using ResApi.DTA.Intefaces;
using ResApi.Models;
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
        public RoleController(IUnitOfWork unitOfWork, IRole role)
        {
            _unitOfWork = unitOfWork;
            _role = role;
        }
        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<Role>> GetRole(int roleId, CancellationToken cancellationToken)
        {
            return Ok(await _role.Get(roleId, cancellationToken));
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<Role>>> GetAllRoles(CancellationToken cancellationToken)
        {
            return Ok(await _role.GetAll(cancellationToken));
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Role>> CreateRole([FromBody] Role entity, CancellationToken cancellationToken)
        {
            _role.Add(entity);
            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<Role>> UpdateRole([FromBody] Role entity, CancellationToken cancellationToken)
        {
            _role.Update(entity);
            await _unitOfWork.Save(cancellationToken);

            return Ok();
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

    }
}

