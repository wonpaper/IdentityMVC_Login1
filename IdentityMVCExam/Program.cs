using IdentityMVCExam.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDb") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

// DbContext 연결, Identity 등록, EF에 DbContext 등록
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<AppIdentityUser,AppIdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
builder.Services.ConfigureApplicationCookie(opt => {
    opt.LoginPath = "/Security/SignIn";                 // 로그인 처리페이지
    opt.AccessDeniedPath = "/Security/AccessDenied";    // 접근권한 없는 User가 접근할때 처리페이지

});

builder.Services.Configure<IdentityOptions>(options =>
{
	// 비밀번호 규칙
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 8;
	options.Password.RequiredUniqueChars = 0;
});



// Add services to the container.
builder.Services.AddControllersWithViews();

#region [1] Session 개체 사용
//[0] 세션 개체 사용: Microsoft.AspNetCore.Session.dll NuGet 패키지 참조 
//services.AddSession(); 
// Session 개체 사용시 옵션 부여 
builder.Services.AddSession(options =>
{
	// 세션 유지 시간
	options.IdleTimeout = TimeSpan.FromMinutes(30);
});
#endregion

/*
#region Authorization

AddAuthorizationPolicies();

#endregion

AddScoped();
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

/*
void AddAuthorizationPolicies()
{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Constants.Policies.RequireAdmin, policy => policy.RequireRole(Constants.Roles.Administrator));
        options.AddPolicy(Constants.Policies.RequireManager, policy => policy.RequireRole(Constants.Roles.Manager));
    });
}

void AddScoped()
{
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
}

*/