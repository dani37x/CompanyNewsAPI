using CompanyNewsAPI.Models.ModelValidators;
using CompanyNewsAPI.Models.Validators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CompanyNewsAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [MinimumLengthValidator(minLength: 3)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        //[MinimumLengthValidator(minLength: 3)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        //[MinimumLengthValidator(minLength: 5)]
        //[EmailValidator(ErrorMessage = "Email must contain dot and at and be in correct format")]
        public string Email { get; set; }
        [Required]
        //[MinimumLengthValidator(minLength: 9)]
        //[PasswordValidator(ErrorMessage = "Password is not in correct format")]
        public string Password { get; set; }
        //[Required]
        //[RoleValidator(ErrorMessage = "")]
        //public string Role { get; set; }

        [JsonIgnore]
        public ICollection<Post>? Posts { get; set; }
    }
}
