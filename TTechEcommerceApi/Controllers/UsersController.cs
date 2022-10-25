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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponseModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(userService.GetAllUsers());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong please contact the support for more information!");
                //todo log errors later on
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Register([FromBody] UserRequestModel model)
        {
            try
            {
                var user = await userService.Register(model);
                return Created("TODO", user);
            }
            catch (TTechException te)
            {
                return BadRequest(te.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong please contact the support for more information!");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequestModel model)
        {
            try
            {
                var response = await userService.Authenticate(model);
                return Ok(response);
            }
            catch (TTechException te)
            {
                return BadRequest(te.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong please contact the support for more information!");
            }
        }

        [HttpPut]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Update([FromRoute] int userId, [FromBody] UserRequestModel model)
        {
            try
            {
                return Ok(await userService.Update(model, userId));
            }
            catch (TTechException te)
            {
                return BadRequest(te.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong please contact the support for more information!");
            }
        }

        [HttpDelete]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete([FromRoute] int userId)
        {
            try
            {
                await userService.Delete(userId);
                return NoContent();
            }
            catch (TTechException te)
            {
                return BadRequest(te.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong please contact the support for more information!");
            }
        }
    }
}
