using System;
using System.ComponentModel.DataAnnotations;

namespace Pdf.Api.Dto.v1
{
    public sealed record ConfirmEmailRequestDto([Required] Guid ConfirmationToken, [Required] string Code);
}