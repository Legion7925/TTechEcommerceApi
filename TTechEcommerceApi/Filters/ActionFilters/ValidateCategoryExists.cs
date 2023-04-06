using EcommerceApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace TTechEcommerceApi.Filters.ActionFilters
{
    public class ValidateCategoryExists : IAsyncActionFilter
    {
        private readonly EcommerceContext dbContext;

        public ValidateCategoryExists(EcommerceContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int categoryId = 0;
            if (context.ActionArguments.ContainsKey("categoryId"))
            {
                categoryId = (int?)context.ActionArguments["categoryId"] ?? 0;
            }
            else
            {
                context.Result = new BadRequestObjectResult("category id parameter not found!!!");
                return;
            }

            var category = await dbContext.Categories.FirstOrDefaultAsync(i => i.Id == categoryId);
            if (category == null) 
            { 
                context.Result = new NotFoundObjectResult("category not found!");
                return;
            }
            await next();
        }
    }
}
