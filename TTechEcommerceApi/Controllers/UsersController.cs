using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTechEcommerceApi.Helper;
using TTechEcommerceApi.Interface;
using TTechEcommerceApi.Model;

namespace TTechEcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserResponseModel>))]
        public IActionResult GetAll()
        {
            return Ok(userService.GetAllUsers());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> Register([FromBody] UserRequestModel model)
        {
            try
            {
                var user = await userService.Register(model);
                return Created("TODO",user );
            }
            catch (TTechException te)
            {
                return BadRequest(te.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong please contact the support for more information!");
            }
        }

        [HttpPost]
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
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong please contact the support for more information!");
            }
        }
    }
}
