using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RealEstateListingPlatform.Data;
using RealEstateListingPlatform.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealEstateListingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            var user = await _userManager.FindByEmailAsync(userLogin.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, userLogin.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
        {
            new Claim("email", user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userId", user.Id), // Add userId claim
        };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim("role", userRole)); // Add role claim
                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized("User doesn't exist");
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                //issuer: _configuration["JWT:Issuer"],
                //audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
        


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegistration userRegistration)
        {
            var userExists = await _userManager.FindByEmailAsync(userRegistration.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });

            var newUser = new User
            {
                UserName = userRegistration.UserName,
                Email = userRegistration.Email,
                PhoneNumber = userRegistration.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(newUser, userRegistration.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            // Check if "User" role exists, and add it if it doesn't
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            // Assign the "User" role to the new user
            await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }

    }
}


