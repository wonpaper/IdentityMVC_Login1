using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace IdentityMVCExam.Models
{
	public class SignIn
	{
		[Required(ErrorMessage="{0}을 입력해 주십시오.")]
		[Display(Name = "사용자이름")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "{0}를 입력해 주십시오.")]
		[Display(Name = "비밀번호")]
		public string Password { get; set; }

		[Required]
		[Display(Name = "사용자저장")]
		public bool RememberMe { get; set; }
	}
}
