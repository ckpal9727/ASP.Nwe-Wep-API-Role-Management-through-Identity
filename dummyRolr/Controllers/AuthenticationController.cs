using dummyRolr.Model;
using dummyRolr.Model.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dummyRolr.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthenticationController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel registerModel,string role)
		{
			if (registerModel == null) {
				return BadRequest();
			}
			//check Exist User
			var userExist =await _userManager.FindByEmailAsync(registerModel.Email);
			if (userExist != null)
			{
				return StatusCode(StatusCodes.Status403Forbidden, new Response { StatusCode ="Error 403",Message="User already exist"});
			}
			//Add user
			IdentityUser user = new()
			{
				Email = registerModel.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = registerModel.Name
			};
			if(await _roleManager.RoleExistsAsync(role))
			{
				var result = await _userManager.CreateAsync(user, registerModel.Password);
				if (!result.Succeeded)
				{
					return StatusCode(StatusCodes.Status403Forbidden, new Response { StatusCode = "Error 403", Message = "User already exist" });
				}
				//Assign A role
				var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Email, registerModel.Email),
					new Claim(ClaimTypes.Role,role),
					new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				};
				var jwtToken = GetToken(authClaims);

				await _userManager.AddToRoleAsync(user, role);
				return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
					expiration = jwtToken.ValidTo,
					
				});
			}
			else
			{
				return StatusCode(StatusCodes.Status403Forbidden, new Response { StatusCode = "Error 403", Message = "The role is exist" });
			}
			
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginModel loginModel)
		{
			// Check the user
			var userExist = await _userManager.FindByEmailAsync(loginModel.Email);

			// Create claims
			if (userExist != null && await _userManager.CheckPasswordAsync(userExist, loginModel.password))
			{
				var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Email, loginModel.Email),
					new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				};

				var userRoles = await _userManager.GetRolesAsync(userExist);

				// Add roles to the claims
				foreach (var role in userRoles)
				{
					authClaims.Add(new Claim(ClaimTypes.Role, role));
				}

				// Generate the token
				var jwtToken = GetToken(authClaims);

				// Return token
				return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
					expiration = jwtToken.ValidTo,
					
				});
			}

			return StatusCode(StatusCodes.Status403Forbidden, new Response { StatusCode = "Error 401", Message = "Invalid email or Password" });
		}

		private JwtSecurityToken GetToken(List<Claim> authClaims)
		{
			var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
			var token = new JwtSecurityToken(
				issuer: _configuration["JwtSettings:Issuer"],
				audience: _configuration["JwtSettings:Audience"],
				expires: DateTime.Now.AddMinutes(1),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
			);
			return token;
		}


	}
}
