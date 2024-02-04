using dummyRolr.DB;
using dummyRolr.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Claims;

namespace dummyRolr.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LeaveController : ControllerBase
	{

		private readonly AppDbContext _appDbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public LeaveController(AppDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
		{
			_appDbContext = appDbContext;
			_httpContextAccessor = httpContextAccessor;
		}

		/*[Authorize(Roles = "Admin")]*/
		[HttpGet]
		public async Task<IActionResult> GetAllLeave()
		{
			if (_appDbContext.Leaves == null)
			{
				return NotFound();	
			}
			return Ok(await _appDbContext.Leaves.ToListAsync());
		}
		[HttpGet("{email}")]
		public async Task<IActionResult> GetMyLeaves(string email)
		{
			if (_appDbContext.Leaves == null)
			{
				return NotFound();
			}
			var userLeaves = await _appDbContext.Leaves
		.Where(leave => leave.UserEmail == email)
		.ToListAsync();

			return Ok(userLeaves);
			
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> ApplyLeave(Leave leave)
		{
			var userEmail=_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;			
			if (userEmail == null)
			{
				return StatusCode(StatusCodes.Status403Forbidden,new Response { StatusCode = "403", Message = "Email is not exist" });
				
			}
			var existLeaveForSpecificType = await _appDbContext.userLeaveBalances
			.Where(x => x.UserEmail == userEmail)
			.FirstOrDefaultAsync();
			if(existLeaveForSpecificType.ExistLeave > 0)
			{
				var newLeave = new Leave
				{
					AppliedDate = DateTime.Now,
					LeaveTypeId = _appDbContext.LeaveTypes.FirstOrDefault().Id,
					StartDate = DateTime.Now,
					EndDate = DateTime.Now,
					Reason = leave.Reason,
					UserEmail = userEmail

				};
				existLeaveForSpecificType.ExistLeave = existLeaveForSpecificType.ExistLeave - 1;

				var appliedLeave = await _appDbContext.Leaves.AddAsync(newLeave);
				await _appDbContext.SaveChangesAsync();
				return Ok(appliedLeave);
			}
			else
			{
				return StatusCode(StatusCodes.Status403Forbidden, new Response { StatusCode = "403", Message = "Not sufficient value" });
			}


		}
		[Authorize]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateLeave(Leave leave,int id)
		{
			var userEmail = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
			var existLeave = await _appDbContext.Leaves.FirstOrDefaultAsync(l => l.UserEmail == userEmail && l.Id == id);
			if (existLeave == null)
			{
				 return StatusCode(StatusCodes.Status403Forbidden, new Response { StatusCode = "403", Message = "Leave is not exist" });
			}

			var existLeaveForSpecificType = await _appDbContext.userLeaveBalances
			.Where(x => x.UserEmail == userEmail)
			
			.FirstOrDefaultAsync();
			if (existLeaveForSpecificType.ExistLeave > 0)
			{
				if (leave.AppliedDate.AddDays(3) > DateTime.Now)
				{
					return StatusCode(StatusCodes.Status403Forbidden, new Response { StatusCode = "403", Message = "Not allowed to edit after 3 days" });
				}
				else
				{
					existLeaveForSpecificType.ExistLeave = existLeaveForSpecificType.ExistLeave - 1;
					var updateLeave = new Leave
					{
						AppliedDate = leave.AppliedDate,
						LeaveTypeId = _appDbContext.LeaveTypes.FirstOrDefault().Id,
						Reason = leave.Reason,
						EndDate = DateTime.Now,
						StartDate = DateTime.Now,
						UserEmail = userEmail,
					};
					_appDbContext.Leaves.Update(existLeave);
					await _appDbContext.SaveChangesAsync();
					return StatusCode(StatusCodes.Status200OK, new Response { StatusCode = "200", Message = "Leave Updated" });


				}

			}
			else
			{
				 return StatusCode(StatusCodes.Status403Forbidden, new Response { StatusCode = "403", Message = "Not sufficient value" });
			}

		}



	}
}
