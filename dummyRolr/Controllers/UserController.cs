using dummyRolr.Model;
using dummyRolr.Model.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dummyRolr.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;

		public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
		}
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> GetAllUser()
		{
			var users = await _userManager.Users.ToListAsync();
			if (users != null)
			{
				return Ok(users);

			}
			else
			{
				return NotFound("No user found");
			}
		}
		[HttpGet("{email}")]
		public async Task<IActionResult> GetSingleUser(string email)
		{

			var users = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (users != null)
			{
				return Ok(users);

			}
			else
			{
				return NotFound("No user found");
			}
		}
		[HttpPut("{email}")]
		public async Task<IActionResult> UpdateUser(RegisterModel user,string email)
		{
			if(user== null)
			{
				return BadRequest();
			}
			var existUser=  await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
			if (existUser != null)
			{
				IdentityUser updateUser = new()
				{
					Email = user.Email,					
					UserName = user.Name
					
				};
				existUser.UserName = updateUser.UserName;
				existUser.Email= user.Email;

				//updating databse
				var result = await _userManager.UpdateAsync(existUser);

				if (result.Succeeded)
				{
					// Successfully updated user
					return StatusCode(StatusCodes.Status200OK, new Response { StatusCode = "200", Message = "User Updated" });
				}
				else
				{
					// Handle errors during the update
					return BadRequest(result.Errors);
				}


			}
			else
			{
				return BadRequest();
			}
			


		}

		[HttpDelete("{email}")]
		public async Task<IActionResult> DeleteUser( string email)
		{
			var existUser = await _userManager.FindByEmailAsync(email);

			if (existUser == null)
			{
				// User with the provided email does not exist
				return NotFound();
			}

			var result = await _userManager.DeleteAsync(existUser);

			if (result.Succeeded)
			{
				// User successfully deleted
				return NoContent();
			}
			else
			{
				// Something went wrong while deleting the user
				return BadRequest(result.Errors);
			}

		}

	}
}
