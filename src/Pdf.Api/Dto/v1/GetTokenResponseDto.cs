using System;

namespace Pdf.Api.Dto.v1
{
    public sealed record GetTokenResponseDto(string AccessToken, string RefreshToken, DateTime Expired);
}