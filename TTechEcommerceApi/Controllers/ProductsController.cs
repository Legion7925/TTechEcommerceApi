using EcommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTechEcommerceApi.Filters.ActionFilters;
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
            return CreatedAtAction(nameof(GetById), new { productId = submittedProduct.Id }, submittedProduct);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{productId}")]
        [ServiceFilter(typeof(ValidateProductExists))]
        public async Task<IActionResult> Update([FromRoute] int productId, [FromBody] Product product)
        {
            await productService.UpdateProduct(productId, product);
            return Ok();
        }

        [HttpGet]
        [Route("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(ValidateProductExists))]
        public async Task<IActionResult> GetById(int productId)
        {
            var product = await productService.GetProductById(productId);
            return Ok(product);
        }

        [HttpDelete]
        [Route("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int productId)
        {
           await productService.DeleteProduct(productId);
           return NoContent();
        }
    }
}
