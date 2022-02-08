namespace Pdf.Api.Exception
{
    internal sealed class CustomerWithEmailAlreadyExistsException : System.Exception
    {
        public CustomerWithEmailAlreadyExistsException(string email)
            : base($"Customer with email {email} already exists")
        {

        }
    }
}