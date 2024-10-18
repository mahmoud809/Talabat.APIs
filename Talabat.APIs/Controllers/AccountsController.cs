using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    
    public class AccountsController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager ,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper
            ) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
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
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
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
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
            
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager) //Because we have not stored token yet in our Db so we generate a new one 
                //[Soon We will know how to store Created token in our Db]
            });
        }


        [Authorize]
        [HttpGet("address")] //GET : /api/accounts/address
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            

            var user = await _userManager.FindUserWithAddressAysnc(User);

            var address = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(address);
        }

        [Authorize]
        [HttpPut("address")] //PUT : /api/accounts/address

        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedaddress)
        {
            var address = _mapper.Map<AddressDto, Address>(updatedaddress);

            var user = await _userManager.FindUserWithAddressAysnc(User);

            address.Id = user.Address.Id;

            user.Address = address;

            var result  = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse(400));
            }
            return Ok(updatedaddress);
        }
    }
}
