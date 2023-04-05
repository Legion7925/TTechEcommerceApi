using EcommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTechEcommerceApi.Helper;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    [ApiController]
    public class ProductsV1Controller : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsV1Controller(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetAll([FromQuery] ProdudctQueryParametersModel queryParameters)
        {
            return Ok(productService.GetAll(queryParameters));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] Product product)
        {
            var submittedProduct = await productService.AddProduct(product);
            return RedirectToAction("GetById", new { productId = submittedProduct.Id });
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{productId}")]
        public async Task<IActionResult> Update([FromRoute] int productId, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var result = await productService.UpdateProduct(productId, product);
            if (result == null)
                return NotFound();
            else
                return Ok(result);

        }

        [HttpGet]
        [Route("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int productId)
        {
            var product = await productService.GetProductById(productId);
            if (product == null)
                return NotFound();
            else
                return Ok(product);
        }

        [HttpDelete]
        [Route("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int productId)
        {
            var success = await productService.DeleteProduct(productId);
            if (!success)
                return NotFound();
            else
                return NoContent();
        }
    }
}
