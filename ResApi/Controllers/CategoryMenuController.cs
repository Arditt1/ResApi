﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class CategoryMenuController : ControllerBase
    {
        private readonly ICategoryMenu _catMenu;
        private readonly ILogger<CategoryMenuController> _logger;
        private readonly IMapper _mapper;
        public CategoryMenuController(ICategoryMenu catMenu,IMapper mapper, ILogger<CategoryMenuController> logger)
        {
            _catMenu = catMenu;
            _mapper = mapper;
            _logger = logger;
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
                Console.WriteLine("Error=", e.Message);
                //_logger.Error(e, "Register POST request");
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
        public async Task<ActionResult<List<CategoryMenuDTO>>> GetAllCategoryMenu(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _catMenu.GetAll(cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                //_logger.Error(e, "Register POST request");
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
                await _catMenu.CreateCategoryMenu(entity);
                return Ok();
            }
            catch (Exception e)
            {
                //_logger.Error(e, "Register POST request");
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
        public async Task<ActionResult<CategoryMenu>> UpdateCategoryMenu([FromBody] CategoryMenuDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                await _catMenu.UpdateCategoryMenu(entity);
                return Ok();
            }
            catch (Exception e)
            {
               // _logger.Error(e, "Register POST request");
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

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult> DeleteCategoryMenu( int id, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _catMenu.Get(id, cancellationToken);
                if (entity == null)
                    return BadRequest("No time log was found with the provided ID.");

                _catMenu.Delete(entity);
                return Ok();
            }
            catch (Exception e)
            {
                //_logger.Error(e, "Register POST request");
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

