﻿using EcommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTechEcommerceApi.Helper;
using TTechEcommerceApi.Interface;

namespace TTechEcommerceApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetAll()
        {
            return Ok(productService.GetAll());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (product.Id < 0)
                return BadRequest("Invalid Id !");

            try
            {
                var submittedProduct = await productService.AddProduct(product);
                return Created("TODO", submittedProduct);
            }
            catch (TTechException te)
            {
                return BadRequest(te.Message);
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
        [Route("{productId}")]
        public async Task<IActionResult> Update([FromRoute] int productId, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var result = await productService.UpdateProduct(productId, product);
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
        [Route("{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int productId)
        {
            try
            {
                var product = await productService.GetProductById(productId);
                if (product == null)
                    return NotFound();
                else
                    return Ok(product);
            }
            catch (Exception ex)
            {
                //todo log the exception
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{productId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int productId)
        {
            try
            {
                var success = await productService.DeleteProduct(productId);
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
