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
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrder _order;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public OrderController(IUnitOfWork unitOfWork, IOrder order)
        {
            _unitOfWork = unitOfWork;
            _order = order;
        }
        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<Order>> GetOrder(int orderId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _order.Get(orderId, cancellationToken);
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
            //return Ok(await _order.Get(orderId, cancellationToken));
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<Order>>> GetAllOrders(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _order.GetAll(cancellationToken);
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
            //return Ok(await _order.GetAll(cancellationToken));
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                var response = _order.CreateOrder(entity);
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
            //_order.Add(entity);
            //await _unitOfWork.Save(cancellationToken);
            //return Ok();
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<Order>> UpdateOrder([FromBody] OrderDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                var response = _order.UpdateOrder(entity);
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

        //[HttpPost]
        //[Route("OrdersToKitchen")]
        //public async Task<ActionResult<Order>> OrdersToKitchen([FromBody] int orderid, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var response = _order.OrdersToKitchen(orderid, cancellationToken);
        //        await _unitOfWork.Save(cancellationToken);
        //        return Ok(response);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.Error(e, "Register POST request");
        //        var errRet = new DataResponse<bool>
        //        {
        //            Succeeded = false,
        //            ErrorMessage = "Error on OrdersToKitchen"

        //        };
        //        return BadRequest(errRet);
        //    }
        //}

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult> DeleteOrder([FromBody] int orderId, CancellationToken cancellationToken)
        {
            var entity = await _order.Get(orderId, cancellationToken);

            if (entity == null)
                return BadRequest("No entity was found with the provided ID.");


            _order.Delete(entity);

            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }
    }
}

