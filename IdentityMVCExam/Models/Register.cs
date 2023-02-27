using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace IdentityMVCExam.Models
{
	public class Register
	{
		[Required(ErrorMessage = "{0}을 입력해 주십시오.")]
		[Display(Name = "사용자이름")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "{0}를 입력해 주십시오.")]
		[Display(Name = "비밀번호")]
		public string Password { get; set; }

		[Required(ErrorMessage = "{0}란을 입력해 주십시오.")]
		[Display(Name = "비밀번호 확인")]
		[Compare("Password", ErrorMessage = "입력하신 비밀번호와 일치하지 않습니다.")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "{0}을 입력해 주십시오.")]
		[Display(Name = "이메일")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "{0}을 입력해 주십시오.")]
		[Display(Name = "사용자이름(전체)")]
		public string FullName { get; set; }

		[Required(ErrorMessage = "{0}을 제대로 선택해 주십시오.")]
		[Display(Name = "생년월일")]
		public DateTime BirthDate { get; set; }
	}
}
