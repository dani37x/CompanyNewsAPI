using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CompanyNewsAPI.Models.ModelValidators
{
    public class PasswordValidator : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {   
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(value.ToString());
            return match.Success;
        }
    }
}
