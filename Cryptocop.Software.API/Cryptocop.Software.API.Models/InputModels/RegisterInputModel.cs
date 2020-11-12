using System.ComponentModel.DataAnnotations;


namespace Cryptocop.Software.API.Models.InputModels
{
    public class RegisterInputModel
    {
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        public string FullName { get; set; }

        [Required]
        [MinLength(8)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string PasswordConfirmation { get; set; }
    }
}