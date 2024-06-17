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
    public class CategoryMenuController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryMenu _catMenu;
        public CategoryMenuController(IUnitOfWork unitOfWork,ICategoryMenu catMenu)
        {
            _unitOfWork = unitOfWork;
            _catMenu = catMenu;
        }
        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<CategoryMenu>> GetCategoryMenu(int CatMenuId, CancellationToken cancellationToken)
        {
            return Ok(await _catMenu.Get(CatMenuId, cancellationToken));
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<CategoryMenu>>> GetAllCategoryMenu(CancellationToken cancellationToken)
        {
            return Ok(await _catMenu.GetAll(cancellationToken));
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<CategoryMenu>> CreateCategoryMenu([FromBody] CategoryMenu entity, CancellationToken cancellationToken)
        {
            _catMenu.Add(entity);
            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<CategoryMenu>> UpdateCategoryMenu([FromBody] CategoryMenu entity, CancellationToken cancellationToken)
        {
            _catMenu.Update(entity);
            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult> DeleteCategoryMenu([FromBody] int CatMenuId, CancellationToken cancellationToken)
        {
            var entity = await _catMenu.Get(CatMenuId, cancellationToken);

            if (entity == null)
                return BadRequest("No time log was found with the provided ID.");


            _catMenu.Delete(entity);

            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }

    }
}

