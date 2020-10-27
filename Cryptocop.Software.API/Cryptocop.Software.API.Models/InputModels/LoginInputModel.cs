using System.ComponentModel.DataAnnotations;


namespace Cryptocop.Software.API.Models.InputModels
{
    public class LoginInputModel
    {
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}