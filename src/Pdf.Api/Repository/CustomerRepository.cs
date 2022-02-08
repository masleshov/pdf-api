using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Pdf.Api.Domain;

namespace Pdf.Api.Repository
{
    internal sealed class CustomerRepository : BaseRepository
    {
        public CustomerRepository(NpgsqlConnection connection) : base(connection)
        {
        }

        public async Task<Customer> GetCustomer(int customerId)
        {
            var query = "select customer_id as CustomerId " +
                        "     , email " + 
                        "     , password_hash as PasswordHash" + 
                        "     , password_salt as PasswordSalt" + 
                        "     , status " + 
                        "     , create_timestamp as CreateTimestamp " + 
                        "     , update_timestamp as UpdateTimestamp " + 
                        "from customer " +
                        "where customer_id = @customerId";
            
            return await Connection.QuerySingleOrDefaultAsync<Customer>(query, new { customerId });
        }

        public async Task<Customer[]> GetCustomer(string email)
        {
            var query = "select customer_id as CustomerId " +
                        "     , email " + 
                        "     , password_hash as PasswordHash" + 
                        "     , password_salt as PasswordSalt" + 
                        "     , status " + 
                        "     , create_timestamp as CreateTimestamp " + 
                        "     , update_timestamp as UpdateTimestamp " + 
                        "from customer " +
                        "where email = @email ";
            
            return (await Connection.QueryAsync<Customer>(query, new { email })).ToArray();
        }

        public async Task<int?> AddCustomerIfNotExists(Customer customer)
        {
            var parameters = new {
                Email = customer.Email,
                PasswordHash = customer.PasswordHash,
                PasswordSalt = customer.PasswordSalt,
                Status = customer.Status
            };

            var query = "insert into customer(email, password_hash, password_salt, status) " +
                        "select @Email, @PasswordHash, @PasswordSalt, @Status " +
                        "where not exists " +
                        "   ( " +
                        "       select null " +
                        "       from customer c " +
                        "       where c.email = @Email" +
                        "   ) " +
                        "returning customer_id";

            return await Connection.QuerySingleOrDefaultAsync<int?>(query, parameters);    
        }

        public async Task SetStatus(int customerId, CustomerStatus status)
        {
            var query = "update customer " +
                        "set status = @status, " + 
                        "    update_timestamp = current_timestamp at time zone 'UTC' " +
                        "where customer_id = @customerId; ";
            
            await Connection.ExecuteAsync(query, new { customerId, status });
        }
    }
}