using System;
using System.ComponentModel.DataAnnotations;

namespace Pdf.Api.Validation
{
    internal sealed class EmailValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value as string;
            if(string.IsNullOrEmpty(email)) throw new ArgumentException($"{nameof(value)} is not a string");

            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith(".")) {
                return new ValidationResult("Email ends on \".\"");
            }
            try {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                if (mailAddress.Address != trimmedEmail)
                {
                    return new ValidationResult($"Wrong email. Received {email}");
                }
            }
            catch {
                return new ValidationResult($"Wrong email. Received {email}");;
            }

            return ValidationResult.Success;
        }
    }
}