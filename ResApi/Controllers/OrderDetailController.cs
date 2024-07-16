using Microsoft.AspNetCore.Mvc;
using NLog;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces;
using ResApi.DTO;
using ResApi.DTO.OrderDetail;
using ResApi.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ResApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderDetail _orderDetail;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public OrderDetailController(IUnitOfWork unitOfWork, IOrderDetail orderDetail)
        {
            _unitOfWork = unitOfWork;
            _orderDetail = orderDetail;
        }
        [HttpGet]
        [Route("getOne")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetail(int orderId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _orderDetail.GetOneOrderDetail(orderId, cancellationToken);
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

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<List<OrderDetail>>> GetAllOrderDetails(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _orderDetail.GetAllOrderDetails(cancellationToken);
                await _unitOfWork.Save(cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
                //_logger.Error(e, "Register POST request");
                var errRet = new DataResponse<bool>
                {
                    Succeeded = false,
                    ErrorMessage = "Error on register user"

                };
                return BadRequest(errRet);
            }
        }
        [HttpPost]
        [Route("orderFood")]
        public async Task<ActionResult<DataResponse<string>>> OrderFood(List<OrderFoodDTO> props, int? tableId, int? waiterId, decimal totalPrice, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _orderDetail.OrderFood(props, tableId, waiterId,totalPrice, cancellationToken);
                if (entity.Succeeded)
                    return Ok(entity);
                else
                    return NotFound();
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
        public async Task<ActionResult<OrderDetail>> CreateOrderDetail([FromBody] OrderDetailDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _orderDetail.CreateOrderDetail(entity);
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
        public async Task<ActionResult<OrderDetail>> UpdateOrderDetail([FromBody] OrderDetailDTO entity, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _orderDetail.UpdateOrderDetail(entity);
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
        public async Task<ActionResult> DeleteOrderDetail([FromBody] int orderDetailId, CancellationToken cancellationToken)
        {
            var entity = await _orderDetail.Get(orderDetailId, cancellationToken);

            if (entity == null)
                return BadRequest("No entity was found with the provided ID.");


            _orderDetail.Delete(entity);

            await _unitOfWork.Save(cancellationToken);

            return Ok();
        }
        [HttpGet]
        [Route("getAllOrderDetails")]
        public async Task<ActionResult<List<GetAllOrderDetailsDTO>>> GetAllOrderDetailsMapped(CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _orderDetail.GetAllOrderDetails(cancellationToken);
                if (entity == null)
                    return NotFound();
                else
                    return Ok(entity);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

