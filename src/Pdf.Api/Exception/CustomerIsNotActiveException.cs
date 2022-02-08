namespace Pdf.Api.Exception
{
    internal sealed class CustomerIsNotActiveException : System.Exception
    {
        public CustomerIsNotActiveException(string email)
            : base($"Customer with email {email} is not active. Confirm email first")
        {

        }
    }
}