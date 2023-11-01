using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        //Fields
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        
        //Constructor
        public AuthController(UserManager<IdentityUser>userManager,ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }
        
        #region Register
        //POST: api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser()
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName
            };
            var identityResult = _userManager.CreateAsync(identityUser, registerRequestDto.Password);
            //Check if the user is created successfully
            if (identityResult.Result.Succeeded)
            {
                //Check if the user has any roles
                if (registerRequestDto.Roles.Any())
                {
                    //Add the roles to the user
                    await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    //Return the response
                    if (identityResult.Result.Succeeded)
                    {
                        return Ok("User created successfully");
                    }
                }
            }
            //Return the response
            return BadRequest("Something went wrong");
        }//end of Register
        #endregion
        
        #region Login
        //POST: api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        { 
            //Get the user from database
           var user= await _userManager.FindByEmailAsync(loginRequestDto.UserName);
           //Check if the user exists
           if(user!=null && await _userManager.CheckPasswordAsync(user,loginRequestDto.Password))
           {
               //Get the roles assigned to the user
                var roles = await _userManager.GetRolesAsync(user);
                //Check if the user has any roles
                if(roles!=null)
                {
                    //Create the token
                    var jwtToken = _tokenRepository.CreateJWTToken(user,roles.ToList());
                    //Create the response
                    var response = new LoginResponseDto()
                    {
                        JwtToken = jwtToken 
                    };
                    //Return the response
                    return Ok(response);
                }
           }
           return BadRequest("Something went wrong");
        }
        #endregion
    }
}