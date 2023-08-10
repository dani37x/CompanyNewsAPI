using System.ComponentModel.DataAnnotations;

namespace CompanyNewsAPI.Models.Validators
{
    public class MinimumLengthValidator : ValidationAttribute
    {
        private int _minLength;

        public MinimumLengthValidator(int minLength)
        {
            this._minLength = minLength;
            this.ErrorMessage = "The field must be at least " + _minLength + " characters long.";
        }

        public override bool IsValid(object value)
        {
            if (!string.IsNullOrWhiteSpace(value.ToString()) && value.ToString().Length >= _minLength)
            {
                return true;
            }
            return false;
        }
    }
}
