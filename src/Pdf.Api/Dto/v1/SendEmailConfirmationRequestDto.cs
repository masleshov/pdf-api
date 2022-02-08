using Pdf.Api.Validation;

namespace Pdf.Api.Dto.v1
{
    public sealed record SendEmailConfirmationRequestDto
    {
        [EmailValidation]
        public string Email { get; init; }
        public string Password { get; init; }
    }
}