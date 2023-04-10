using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTechEcommerceApi.Helper;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<UserResponseModel>> GetAll()
        {
            return Ok(userService.GetAllUsers());
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> Register([FromBody] UserRequestModel model)
        {
            var user = await userService.Register(model);
            return Created("TODO", user);
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticateResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequestModel model)
        {
            var response = await userService.Authenticate(model);
            return Ok(response);
        }

        [HttpPut]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> Update([FromRoute] int userId, [FromBody] UserRequestModel model)
        {
            return Ok(await userService.Update(model, userId));
        }

        [HttpDelete]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromRoute] int userId)
        {
            await userService.Delete(userId);
            return NoContent();
        }
    }
}
