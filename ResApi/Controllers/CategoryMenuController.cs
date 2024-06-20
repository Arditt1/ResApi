using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class CategoryMenuController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryMenu _catMenu;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public CategoryMenuController(IUnitOfWork unitOfWork,ICategoryMenu catMenu)
        {
            _unitOfWork = unitOfWork;
            _catMenu = catMenu;
        }
        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<CategoryMenu>> GetCategoryMenu(int CatMenuId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _catMenu.Get(CatMenuId, cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register Category"

                };
                return BadRequest(errRet);
            }
           // return Ok(await _catMenu.Get(CatMenuId, cancellationToken));
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<CategoryMenu>>> GetAllCategoryMenu(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _catMenu.GetAll(cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register Category"

                };
                return BadRequest(errRet);
            }
            //return Ok(await _catMenu.GetAll(cancellationToken));
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<CategoryMenu>> CreateCategoryMenu([FromBody] CategoryMenu entity, CancellationToken cancellationToken)
        {
            try
            {
                _catMenu.Add(entity);
                await _unitOfWork.Save(cancellationToken);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register Category"

                };
                return BadRequest(errRet);
            }
            //_catMenu.Add(entity);
            //await _unitOfWork.Save(cancellationToken);
            //return Ok();
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<CategoryMenu>> UpdateCategoryMenu([FromBody] CategoryMenu entity, CancellationToken cancellationToken)
        {
            try
            {
                _catMenu.Update(entity);
                await _unitOfWork.Save(cancellationToken);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register Category"

                };
                return BadRequest(errRet);
            }
            //_catMenu.Update(entity);
            //await _unitOfWork.Save(cancellationToken);
            //return Ok();
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult> DeleteCategoryMenu([FromBody] int CatMenuId, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _catMenu.Get(CatMenuId, cancellationToken);
                if (entity == null)
                    return BadRequest("No time log was found with the provided ID.");

                _catMenu.Delete(entity);
                await _unitOfWork.Save(cancellationToken);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register Category"

                };
                return BadRequest(errRet);
            }

        }

    }
}

