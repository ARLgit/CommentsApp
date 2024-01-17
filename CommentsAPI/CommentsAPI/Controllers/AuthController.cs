using AutoMapper;
using Azure.Core;
using CommentsAPI.Entities;
using CommentsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CommentsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        /*private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _userEmailStore;*/
        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IConfiguration configuration
            )
        {
            _userManager = userManager ??
                throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ??
                throw new ArgumentNullException(nameof(roleManager));
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }

        // POST 
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO newUser)
        {
            var userExists = await _userManager.FindByEmailAsync(newUser.Email);
            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "El Usuario ya se encuentra registrado." });
            }
            ApplicationUser user = new()
            {
                Email = newUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = newUser.UserName
            };
            var result = await _userManager.CreateAsync(user, newUser.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response<IEnumerable<IdentityError>> { Status = "Error", Value = result.Errors, Message = "El Usuario no ha podido registrarse." });
            }
            await _userManager.AddToRoleAsync(user, "User");
            return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "El Usuario ha side creado exitosamente." });
        }

        // POST
        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LogInRequestDTO userData)
        {
            var user = await _userManager.FindByNameAsync(userData.UserName);
            if (user is null || !await _userManager.CheckPasswordAsync(user, userData.Password))
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new Response { Status = "Error", Message = "El Usuario no existe o la contraseña es incorrecta." });
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(720),
                signingCredentials: signingCredentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return StatusCode(StatusCodes.Status200OK,
                    new Response<string>()
                    {
                        Status = "Success",
                        Value = jwt,
                        Message = "sesión iniciada correctamente."
                    });
        }

        // PUT api/<AuthController>/5
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Failure", Message = "Usuario no valido." });
            }
            //Get Sid from claims
            var id = User.FindFirstValue(ClaimTypes.Sid);
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "Su Token no cuenta con un Sid." });
            }
            //get user by id
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "El Usuario no existe." });
            }
            //update the user and check result
            user.Email = updatedUser.Email;
            user.UserName = updatedUser.UserName;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Failure", Message = "Usuario no pudo ser actualizado." });
            }
            return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "El Usuario ha side actualizado exitosamente." });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO passwords)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Failure", Message = "Datos no validos." });
            }
            //Get Sid from claims
            var id = User.FindFirstValue(ClaimTypes.Sid);
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "Su Token no cuenta con un Sid." });
            }
            //get user by id
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "El Usuario no existe." });
            }
            //change the password and check result
            var updateResult = await _userManager.ChangePasswordAsync(user, passwords.OldPassword, passwords.NewPassword);
            if (!updateResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Failure", Message = "La contraseña no pudo se cambiada." });
            }
            return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "La contraseña se ha cambiado exitosamente." });
        }

        // DELETE api/<AuthController>/5
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] string password)
        {
            //get user id from Sid claim.
            var id = User.FindFirstValue(ClaimTypes.Sid);
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Error", Message = "Su Token no cuenta con un Sid." });
            }
            //get user by id
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "El Usuario no existe." });
            }
            //delete the user
            if (!string.IsNullOrEmpty(password) && await _userManager.CheckPasswordAsync(user, password))
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded) 
                {
                    return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = "El Usuario ha side borrado exitosamente." });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                    new Response { Status = "Failure", Message = "No se ha podido borrar el usuario." });
        }
    }
}
