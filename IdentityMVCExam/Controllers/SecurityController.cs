using IdentityMVCExam.Models;
using IdentityMVCExam.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMVCExam.Controllers
{
	public class SecurityController : Controller
	{
		private readonly UserManager<AppIdentityUser> userManager;
		private readonly RoleManager<AppIdentityRole> roleManager;
		private readonly SignInManager<AppIdentityUser> signinManager;

		public SecurityController(UserManager<AppIdentityUser> userManager, RoleManager<AppIdentityRole> roleManager,
			SignInManager<AppIdentityUser> signinManager)
		{
			this.userManager = userManager;
			this.roleManager = roleManager;
			this.signinManager = signinManager;
		}

		// 회원가입
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Register(Register obj)
		{
			if (ModelState.IsValid)
			{
				// 최초 회원가입시 Manager Role이 없으면 Manager Role 값을 생성해준다.
				if (!roleManager.RoleExistsAsync("Manager").Result)
				{
					AppIdentityRole role = new AppIdentityRole();
					role.Name = "Manager";
					role.Description = "Can perform CRUD operations.";
					IdentityResult roleResult = roleManager.CreateAsync(role).Result;
				}

				AppIdentityUser user = new AppIdentityUser();
				user.UserName = obj.UserName;
				user.Email = obj.Email;
				user.FullName = obj.FullName;
				user.BirthDate = obj.BirthDate;

				// AppIdentityUser 생성한다.
				IdentityResult result = userManager.CreateAsync(user, obj.Password).Result;
				if (result.Succeeded)
				{
					// User 에 Manager Role을 추가해준다. 이때 Wait() 는 비동기처리로 진행이 완료될때까지 잠시 기다려준다.
					userManager.AddToRoleAsync(user, "Manager").Wait();
					return RedirectToAction("SignIn", "Security");
				}
				else
				{
					ModelState.AddModelError("", "회원가입 입력 항목을 모두 정확하게 입력해 주십시오.");
				}
			}
			return View(obj);
		}


		// 로그인
		public IActionResult SignIn()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult SignIn(SignIn obj)
		{
			if (ModelState.IsValid)
			{
				var result = signinManager.PasswordSignInAsync(obj.UserName, obj.Password,obj.RememberMe, false).Result;
				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "로그인 사용자 정보가 맞지 않습니다.");
				}
			}
			return View(obj);
		}

		// 로그아웃

		// 로그아웃
		[HttpGet]
		public IActionResult SignOut()
		{
			signinManager.SignOutAsync().Wait();
			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public IActionResult SignOutPost()
		{
			signinManager.SignOutAsync().Wait();
			return RedirectToAction("Index", "Home");
		}


		// 접근허용금지 처리

		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
