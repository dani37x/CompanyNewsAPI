using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CompanyNewsAPI.Models.Validators
{
    public class EmailValidator : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var email = value.ToString().Split('.');

            if (value.ToString().Contains('@') && value.ToString().Contains('.') && email.Last().Length > 1)
            {
                return true;
            }
           return false;
        }
    }
}
