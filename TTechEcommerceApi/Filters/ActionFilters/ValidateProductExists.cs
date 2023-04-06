using EcommerceApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace TTechEcommerceApi.Filters.ActionFilters
{
    public class ValidateProductExists : IAsyncActionFilter
    {
        private readonly EcommerceContext dbcontext;

        public ValidateProductExists(EcommerceContext ecommerceContext)
        {
            dbcontext = ecommerceContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            int productId = 0;
            if (context.ActionArguments.ContainsKey("productId"))
            {
                productId = (int?)context.ActionArguments["productId"] ?? 0;
            }
            else
            {
                context.Result = new BadRequestObjectResult("product id parameter not found!!!");
                return;
            }

            var product = await dbcontext.Products.FirstOrDefaultAsync(i => i.Id == productId);
            if (product == null)
            {
                context.Result = new NotFoundObjectResult("product not found!");
                return;
            }
            await next();
        }
    }
}
