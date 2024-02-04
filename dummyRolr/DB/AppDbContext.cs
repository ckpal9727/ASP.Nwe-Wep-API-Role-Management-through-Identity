using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dummyRolr.DB
{
	public class AppDbContext : IdentityDbContext<IdentityUser>
	{
		public AppDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Leave> Leaves { get; set; }
		public DbSet<LeaveType> LeaveTypes { get; set; }

		public DbSet<UserLeaveBalance> userLeaveBalances { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			SeedRoles(builder);
		}
		private void SeedRoles(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<IdentityRole>().HasData(

				new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
				new IdentityRole() { Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" }
				);
		}
	}

}
