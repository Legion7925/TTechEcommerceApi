using EcommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Order>))]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(orderService.GetOrders());
            }
            catch (Exception ex)
            {
                //todo log the exception
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (order.Id < 0)
                return BadRequest("Invalid Id !");

            try
            {
                var submittedOrder = await orderService.AddOrder(order);
                return Created("TODO", submittedOrder);
            }
            catch (Exception ex)
            {
                //todo log the exception
                return StatusCode(500);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{categoryId}")]
        public async Task<IActionResult> Update([FromRoute] int orderId, [FromBody] OrderUpdateModel order)
        {
            try
            {
                var result = await orderService.UpdateOrder(order, orderId);
                if (result == null)
                    return NotFound();
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                //todo log the exception
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int orderId)
        {
            try
            {
                var result = await orderService.GetOrderById(orderId);
                if (result == null)
                    return NotFound();
                else
                    return Ok(result);
            }
            catch (Exception ex)
            {
                //todo log the exception
                return StatusCode(500);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int orderId)
        {
            try
            {
                var success = await orderService.DeleteOrder(orderId);
                if (!success)
                    return NotFound();
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                //todo log the exception
                return StatusCode(500);
            }

        }

    }
}
