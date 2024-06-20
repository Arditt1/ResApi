using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces;
using ResApi.DTO;
using ResApi.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployee _emp;
        private readonly ILogger<Employee> _logger;
        public EmployeeController(IUnitOfWork unitOfWork, IEmployee emp,ILogger<Employee> logger)
        {
            _unitOfWork = unitOfWork;
            _emp = emp;
            _logger = logger;
        }
        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<Employee>> GetEmployee(int empId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _emp.Get(empId, cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register user"

                };
                return BadRequest(errRet);
            }
            //return Ok(await _emp.Get(empId, cancellationToken));
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _emp.GetAll(cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register user"

                };
                return BadRequest(errRet);
            }
            //return Ok(await _emp.GetAll(cancellationToken));
        }

        [HttpGet]
        [Route("getKamarierat")]
        public async Task<ActionResult<List<EmployeeDTO>>> GetAllKamarierat(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _emp.GetAllKamarierat(cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var Error = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Problem me marrjen e te dhenave per kamarieret"
                };
                return BadRequest(Error);
            }
        }


        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<EmployeeDTO>> CreateEmployee([FromBody] EmployeeDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _emp.Register(entity);
                await _unitOfWork.Save(cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Register POST request");
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
        public async Task<ActionResult<EmployeeDTO>> UpdateEmployee([FromBody] EmployeeDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                var entity1 = await _emp.Get(entity.Id, cancellationToken);

                if (entity1 == null)
                    return BadRequest("No entity was found with the provided ID.");
                var response = await _emp.UpdateEmployee(entity);
                await _unitOfWork.Save(cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Register POST request");
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
        public async Task<ActionResult> DeleteEmployee([FromBody] int empId, CancellationToken cancellationToken)
        {
            var entity = await _emp.Get(empId, cancellationToken);

            if (entity == null)
                return BadRequest("No entity was found with the provided ID.");


            _emp.Delete(entity);

            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

        [HttpPost]
        [Route("changepass")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto entity, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _emp.ChangePassword(entity, cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register user"

                };
                return BadRequest(errRet);
            }
        }

        [HttpPost]
        [Route("disable")]
        public async Task<ActionResult> DisableEmployee([FromBody] DisableEmployeeDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                var entity1 = await _emp.Get(entity.Id, cancellationToken);

                if (entity1 == null)
                    return BadRequest("No entity was found with the provided ID.");
                var response = await _emp.DisableEmployee(entity, cancellationToken);
                await _unitOfWork.Save(cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register user"

                };
                return BadRequest(errRet);
            }
        }
    }
}

