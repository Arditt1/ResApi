using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NLog;
using ResApi.DataResponse;
using ResApi.DTA.Intefaces;
using ResApi.DTO;
using ResApi.DTO.LoginDTO;
using ResApi.DTO.OrderDetail;
using ResApi.Hubs;
using ResApi.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
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
        private readonly IHubContext<OrderHub> _hubContext;

        public OrderDetailController(IUnitOfWork unitOfWork, IOrderDetail orderDetail, IHubContext<OrderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _orderDetail = orderDetail;
            _hubContext = hubContext;
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
        [Authorize]
        public async Task<ActionResult<List<OrderDetail>>> GetAllOrderDetails(CancellationToken cancellationToken)
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.Name)?.Value;
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if(string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "Invalid token" });

                UserDTO user = new()
                {
                    Id = Convert.ToInt32(userId),
                    Username = username,
                    RoleName = role,
                };

                var response = await _orderDetail.GetAllOrderDetails(user, cancellationToken);
                await _unitOfWork.Save(cancellationToken);
                return Ok(response);
            }
            catch (Exception e)
            {
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
                {
                    await _hubContext.Clients.All.SendAsync("NewOrder", entity.Data);
                    return Ok(entity);
                }
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
        [HttpPut]
        [Route("changeStatus")]
        public async Task<ActionResult<DataResponse<string>>> ChangeOrdersStatus(int orderId, string statusName)
        {
            try
            {
                var result = await _orderDetail.ChangeOrdersStatus(orderId, statusName);
                if (result.Succeeded)
                {
                    await _hubContext.Clients.All.SendAsync("changeOrder", result.Data);

                    return Ok(result);
                }
                else
                    return result;
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Register POST request");
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
        //[HttpGet]
        //[Route("getAllOrderDetails")]
        //public async Task<ActionResult<List<GetAllOrderDetailsDTO>>> GetAllOrderDetailsMapped(CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var entity = await _orderDetail.GetAllOrderDetails(cancellationToken);
        //        if (entity == null)
        //            return NotFound();
        //        else
        //            return Ok(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}

