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
            return Ok(categoryService.GetAll());
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

            var submittedCategory = await categoryService.AddCategory(category);
            return Created("TODO", submittedCategory);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{categoryId}")]
        public async Task<IActionResult> Update([FromRoute] int categoryId, [FromBody] Category category)
        {
            var result = await categoryService.UpdateCategory(categoryId, category);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [HttpGet]
        [Route("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int categoryId)
        {
            var category = await categoryService.GetCategoryById(categoryId);
            if (category == null)
                return NotFound();
            else
                return Ok(category);
        }

        [HttpDelete]
        [Route("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var success = await categoryService.DeleteCategory(categoryId);
            if (!success)
                return NotFound();
            else
                return NoContent();
        }
    }
}
