namespace Pdf.Api.Exception
{
    internal sealed class CustomerDoesNotExistException : System.Exception
    {
        public CustomerDoesNotExistException(string email)
            : base($"Customer with email {email} does not exist")
        {

        }
    }
}