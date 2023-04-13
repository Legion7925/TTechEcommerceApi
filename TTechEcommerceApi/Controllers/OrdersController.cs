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
        public IActionResult GetAll([FromQuery] QueryParametersModel queryParameters)
        {
            return Ok(orderService.GetOrders(queryParameters));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] Order order)
        {
            var submittedOrder = await orderService.AddOrder(order);
            return CreatedAtAction(nameof(GetById) , new { orderId  = submittedOrder.Id} , submittedOrder);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{orderId}")]
        public async Task<IActionResult> Update([FromRoute] int orderId, [FromBody] OrderUpdateModel order)
        {
            var result = await orderService.UpdateOrder(order, orderId);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [HttpGet]
        [Route("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int orderId)
        {
            var result = await orderService.GetOrderById(orderId);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{orderId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int orderId)
        {
            var success = await orderService.DeleteOrder(orderId);
            if (!success)
                return NotFound();
            else
                return NoContent();
        }

    }
}
