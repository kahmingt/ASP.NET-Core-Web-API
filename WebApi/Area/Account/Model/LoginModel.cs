using System.ComponentModel.DataAnnotations;

namespace WebApi.Area.Account.Model
{
    public class AccountLoginModel
    {
        [Required]
        [Display(Name = "Username or email")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}