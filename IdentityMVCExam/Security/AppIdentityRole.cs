using Microsoft.AspNetCore.Identity;

namespace IdentityMVCExam.Security
{
	public class AppIdentityRole : IdentityRole
	{
		public string Description { get; set; } = string.Empty;
	}
}
