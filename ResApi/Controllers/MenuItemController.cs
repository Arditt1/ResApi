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
    public class MenuItemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMenuItem _iMenuItem;
        public MenuItemController(IUnitOfWork unitOfWork,IMenuItem menuItem)
        {
            _unitOfWork = unitOfWork;
            _iMenuItem = menuItem;
        }

        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<MenuItem>> GetMenuItem(int menuItemId, CancellationToken cancellationToken)
        {
            return Ok(await _iMenuItem.Get(menuItemId, cancellationToken));
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<MenuItem>>> GetAllMenuItems(CancellationToken cancellationToken)
        {
            return Ok(await _iMenuItem.GetAll(cancellationToken));
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<MenuItem>> CreateMenuItem([FromBody] MenuItem entity, CancellationToken cancellationToken)
        {
            _iMenuItem.Add(entity);
            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<MenuItem>> UpdateMenuItem([FromBody] MenuItem entity, CancellationToken cancellationToken)
        {
            _iMenuItem.Update(entity);
            await _unitOfWork.Save(cancellationToken);

            return Ok();
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

            return Ok();
        }
    }
}

