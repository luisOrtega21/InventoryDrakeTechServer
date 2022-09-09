using INVENTORY.SERVER.Extensions;
using INVENTORY.SERVER.Services.Interfaces;
using INVENTORY.SHARED.Dto;
using Microsoft.AspNetCore.Mvc;

namespace INVENTORY.SERVER.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userservice;
        public UserController(IUserService userService)
        {
            _userservice = userService;
        }
        //GET: api/User
        [HttpGet]
        [Route("Users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var result = await _userservice.GetUserAsync();
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Entity);
        }

        // POST: api/User
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegister userRegister)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _userservice.RegisterUserAsync(userRegister);

            if (!result.Success)
                return BadRequest(result.Message);

            return CreatedAtAction("GetUserById", new { id = result.Entity.Id }, result);
        }

        //GET: api/UserById
        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var result = await _userservice.GetUserById(userId);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Entity);
        }

        // POST: api/Login
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(UserLogin userLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _userservice.LoginAsync(userLogin);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        //POST/api/ForgotPasword
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _userservice.ForgotPasswordAsync(forgotPasswordModel);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        //PUT/api/ChangePassword
        [HttpPut]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _userservice.ChangePassword(changePassword);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok();
        }
    }
}


