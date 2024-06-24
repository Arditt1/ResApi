using Microsoft.AspNetCore.Mvc;
using NLog;
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
    public class MenuItemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMenuItem _iMenuItem;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public MenuItemController(IUnitOfWork unitOfWork,IMenuItem menuItem)
        {
            _unitOfWork = unitOfWork;
            _iMenuItem = menuItem;
        }

        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<MenuItem>> GetMenuItem(int menuItemId, CancellationToken cancellationToken)
        {
            try
            {
                var response = _iMenuItem.Get(menuItemId, cancellationToken);
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
        public async Task<ActionResult<List<MenuItem>>> GetAllMenuItems(CancellationToken cancellationToken)
        {
            try
            {
                var response = _iMenuItem.GetAll(cancellationToken);
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
            //return Ok(await _iMenuItem.GetAll(cancellationToken));
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<MenuItem>> CreateMenuItem([FromBody] MenuItemDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _iMenuItem.Register(entity);
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
        public async Task<DataResponse<string>> Add(MenuItemDTO entity)
        {
            try
            {
                var addingMenuItem = await _iMenuItem.Add(entity);
                return addingMenuItem;
            }
            catch 
            {
                throw;
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<MenuItem>> UpdateMenuItem([FromBody] MenuItemDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                var entity1 = await _iMenuItem.Get(entity.CategoryId, cancellationToken);

                if (entity1 == null)
                    return BadRequest("No entity was found with the provided ID.");
                var response = await _iMenuItem.UpdateMenuItem(entity);
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
        public async Task<ActionResult> DeleteMenuItem([FromBody] int menuItemId, CancellationToken cancellationToken)
        {
            var entity = await _iMenuItem.Get(menuItemId, cancellationToken);

            if (entity == null)
                return BadRequest("No entity was found with the provided ID.");


            _iMenuItem.Delete(entity);

            await _unitOfWork.Save(cancellationToken);
            //asd
            return Ok();//
        }

        [HttpGet]
        [Route("getMenuItemsWithCategories")]
        public async Task<ActionResult<List<MenuItemDTO>>> GetAllMenuItemsWithCategories(CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _iMenuItem.GetAllMenuItems(cancellationToken);
                if (entity == null)
                    return BadRequest("No menu items found!");

                return Ok(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error=", ex.Message);
                throw;
            }
        }
        [HttpGet]
        [Route("getAllMenuItemsByCategory")]
        public async Task<List<MenuItemDTO>> GetAllMenuItemsByCategory(int CategoryId)
        {
            var entity = await _iMenuItem.GetAllMenuItemsByCategory(CategoryId);
            return entity;
        }

    }
}

