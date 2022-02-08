using System;

namespace Pdf.Api.Infrastructure
{
    public sealed record RefreshToken
    {
        public readonly Guid Token;
        public readonly DateTime Expires;

        public RefreshToken(DateTime expires)
        {
            Token = Guid.NewGuid();
            Expires = expires;
        }
    }
}