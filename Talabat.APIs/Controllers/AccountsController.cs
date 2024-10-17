using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Controllers
{
    
    public class AccountsController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountsController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")] //POST : /api/accounts/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user is null) return Unauthorized(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "This token will be generated soon..!"
            });

        }

        [HttpPost("register")] //POST : /api/accounts/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email, //mahmoud.salah@gmail.com
                UserName = model.Email.Split('@')[0], //mahmoud.salah
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "This token will be generated soon..!"
            });
            
        }
    }
}
