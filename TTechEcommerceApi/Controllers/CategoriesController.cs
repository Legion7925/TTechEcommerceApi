using EcommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTechEcommerceApi.Interface;

namespace TTechEcommerceApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(categoryService.GetAll());
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
        public async Task<IActionResult> Add([FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (category.Id < 0)
                return BadRequest("Invalid Id !");

            try
            {
                var submittedCategory = await categoryService.AddCategory(category);
                return Created("TODO", submittedCategory);
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
        public async Task<IActionResult> Update([FromRoute] int categoryId, [FromBody] Category category)
        {
            try
            {
                var result = await categoryService.UpdateCategory(categoryId, category);
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
        [Route("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int categoryId)
        {
            try
            {
                var category = await categoryService.GetCategoryById(categoryId);
                if (category == null)
                    return NotFound();
                else
                    return Ok(category);
            }
            catch (Exception ex)
            {
                //todo log the exception
                return StatusCode(500);
            }

        }

        [HttpDelete]
        [Route("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int categoryId)
        {
            try
            {
                var success = await categoryService.DeleteCategory(categoryId);
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
