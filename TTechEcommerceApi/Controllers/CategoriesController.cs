using EcommerceApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TTechEcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly EcommerceContext context;

        public CategoriesController(EcommerceContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetAll()
        {
            return Ok(context.Categories.AsNoTracking());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Category))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] Category category)
        {
            if (category.Id < 0)
            {
                return BadRequest("Invalid Id !");
            }
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return Created("TODO", category);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var category = await context.Categories.FirstOrDefaultAsync(i => i.Id == categoryId);
            if (category == null)
                return NotFound();

            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
