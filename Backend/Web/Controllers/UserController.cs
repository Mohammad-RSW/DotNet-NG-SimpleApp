using Microsoft.AspNetCore.Mvc;
using Service.Interfaces.Users;
using Service.DTOs.Users;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly Dictionary<int, Func<object, IActionResult>> responseMapper;

        public UserController(IUserService userService)
        {
            _userService = userService;
            responseMapper = new()
            {
                { 200, (result) => Ok(result) },
                { 201, (result) => StatusCode(201, result) },
                { 204, (result) => StatusCode(204, result) },
                { 400, (result) => BadRequest(result) },
                { 404, (result) => NotFound(result) },
                { 408, (result) => StatusCode(408, result)},
                { 409, (result) => Conflict(result) },
                { 500, (result) => StatusCode(500) },
            }
            ;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAll();
            return responseMapper[result.StatusCode](result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById()
        {
            // dont get User instances, make a dto (maybe just profile)
            // setup jwt for id
            int id = 1;
            var result = await _userService.GetById(id);
            return responseMapper[result.StatusCode](result);
        }

        [HttpGet]
        [Route("profiles")]
        public async Task<IActionResult> GetAllProfiles()
        {
            var result = await _userService.GetAllProfiles();
            return responseMapper[result.StatusCode](result);
        }

        [HttpGet]
        [Route("profile")]
        public async Task<IActionResult> GetUserProfileDetailedById()
        {
            // dont get User instances, make a dto (maybe just profile)
            // setup jwt for id
            int id = 1;
            var result = await _userService.GetUserProfileDetailedById(id);
            return responseMapper[result.StatusCode](result);
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<IActionResult> GetUserProfileSummaryByUsername([FromRoute]string username)
        {
            var result = await _userService.GetUserProfileSummaryByUsername(username);
            return responseMapper[result.StatusCode](result);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser(UserRegisterDto userRegisterDto)
        {
            var result = await _userService.RegisterUser(userRegisterDto);
            return responseMapper[result.StatusCode](result);
        }

        [HttpPut]
        [Route("Edit")]
        public async Task<IActionResult> EditUser(UserEditDto userEditDto)
        {
            //userEditDto.Id = base.User.Identity.Name;
            userEditDto.Id = 2;
            var result = await _userService.EditUser(userEditDto);
            return responseMapper[result.StatusCode](result);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteUserById()
        {
            //id = base.User.Identity.Name;
            int id = 2;
            var result = await _userService.DeleteById(id);
            return responseMapper[result.StatusCode](result);

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUser(UserLoginDto userLoginDto)
        {
            var result = await _userService.LoginUser(userLoginDto);
            return responseMapper[result.StatusCode](result);
        }
    }
}
