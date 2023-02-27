﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityMVCExam.Security
{
	public class AppIdentityDbContext : IdentityDbContext<AppIdentityUser,AppIdentityRole,string>
	{
		public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
		{

		}
	}
}
