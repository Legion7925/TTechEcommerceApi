using EcommerceApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTechEcommerceApi.Filters.ActionFilters;
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
            var submittedCategory = await categoryService.AddCategory(category);
            return CreatedAtAction(nameof(GetById), new { categoryId = submittedCategory.Id } , submittedCategory);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{categoryId}")]
        [ServiceFilter(typeof(ValidateCategoryExists))]
        public async Task<IActionResult> Update([FromRoute] int categoryId, [FromBody] Category category)
        {
            await categoryService.UpdateCategory(categoryId, category);
            return RedirectToAction(nameof(GetById), new { categoryId = categoryId });
        }

        [HttpGet]
        [Route("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(ValidateCategoryExists))]
        public async Task<IActionResult> GetById(int categoryId)
        {
            var category = await categoryService.GetCategoryById(categoryId);
            return Ok(category);
        }

        [HttpDelete]
        [Route("{categoryId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(ValidateCategoryExists))]
        public async Task<IActionResult> Delete(int categoryId)
        {
            await categoryService.DeleteCategory(categoryId);
            return NoContent();
        }
    }
}
