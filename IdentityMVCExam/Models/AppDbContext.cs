using Microsoft.EntityFrameworkCore;

namespace IdentityMVCExam.Models
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
	}
}
