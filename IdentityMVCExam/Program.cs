using IdentityMVCExam.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDb") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

// DbContext ����, Identity ���, EF�� DbContext ���
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<AppIdentityUser,AppIdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
builder.Services.ConfigureApplicationCookie(opt => {
    opt.LoginPath = "/Security/SignIn";                 // �α��� ó��������
    opt.AccessDeniedPath = "/Security/AccessDenied";    // ���ٱ��� ���� User�� �����Ҷ� ó��������

});

builder.Services.Configure<IdentityOptions>(options =>
{
	// ��й�ȣ ��Ģ
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 8;
	options.Password.RequiredUniqueChars = 0;
});



// Add services to the container.
builder.Services.AddControllersWithViews();

#region [1] Session ��ü ���
//[0] ���� ��ü ���: Microsoft.AspNetCore.Session.dll NuGet ��Ű�� ���� 
//services.AddSession(); 
// Session ��ü ���� �ɼ� �ο� 
builder.Services.AddSession(options =>
{
	// ���� ���� �ð�
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