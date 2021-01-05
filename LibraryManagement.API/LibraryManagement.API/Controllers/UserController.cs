using LM.BAL.Interface;
using LM.DAL.Implement;
using LM.Domain.Request.User;
using LM.Domain.Response.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserService userService;

        public UserController(SignInManager<ApplicationUser> signInManager,
                                 IUserService userService)
        {
            this.signInManager = signInManager;
            this.userService = userService;
        }
        [HttpPost]
        [Route("/api/user/login")]
        public async Task<OkObjectResult> Login(LoginReq request)
        {
            var result = await userService.Login(request);
            return Ok(result);
        }
        [HttpGet("api/user/gets")]
        public async Task<OkObjectResult> Gets()
        {
            var users = await userService.Gets();
            return Ok(users);
        }
        [HttpGet("api/user/get/{id}")]
        public async Task<OkObjectResult> Get(string id)
        {
            var user = await userService.Get(id);
            return Ok(user);
        }
        [HttpPost]
        [Route("/api/user/create")]
        public async Task<OkObjectResult> Create(SaveUserReq request)
        {
            var result = await userService.Create(request);
            return Ok(result);
        }
        [HttpPatch]
        [Route("/api/user/edit")]
        public async Task<OkObjectResult> Edit(SaveUserReq request)
        {
            var result = await userService.Edit(request);
            return Ok(result);
        }
        [HttpPatch]
        [Route("api/user/changeStatus")]
        public async Task<OkObjectResult> ChangeStatus(StatusUserReq request)
        {
            var result = await userService.ChangeStatus(request);
            return Ok(result);
        }
        [HttpPost]
        [Route("/api/user/logOut")]
        public async Task<OkObjectResult> Logout()
        {
            await signInManager.SignOutAsync();
            Response.Cookies.Delete("userId");
            var result = new LogOutRes()
            {
                IsSuccess = true
            };
            return Ok(result);
        }
    }
}
