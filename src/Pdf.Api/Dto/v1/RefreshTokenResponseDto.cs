using System;

namespace Pdf.Api.Dto.v1
{
    public sealed record RefreshTokenResponseDto(string AccessToken, string RefreshToken, DateTime Expired);
}