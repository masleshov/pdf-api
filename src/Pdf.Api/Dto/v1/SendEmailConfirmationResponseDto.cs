using System;

namespace Pdf.Api.Dto.v1
{
    public sealed record SendEmailConfirmationResponseDto(Guid ConfirmationToken, int Ttl);
}