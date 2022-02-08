using System;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Pdf.Api.Domain;
using Pdf.Api.Extension;

namespace Pdf.Api.Repository
{
    internal sealed class EmailConfirmationRepository : BaseRepository
    {
        public EmailConfirmationRepository(NpgsqlConnection connection) : base(connection)
        {
        }

        public async Task<Guid?> AddConfirmationIfNotExistsAsync(EmailConfirmation confirmation)
        {
            var parameters = new { 
                CustomerId = confirmation.CustomerId,
                Email = confirmation.Email,
                Code = confirmation.Code,
                Expired = confirmation.Expired,
                Attempts = (int)confirmation.Attempts,
                StatusPending = EmailConfirmationStatus.Pending,
                StatusRejected = EmailConfirmationStatus.Rejected,
            };

            var rejectActiveConfirmationsQuery = @"update email_confirmation " +
                    "set status=@StatusRejected " +
                    "where email = @Email " +
                        " and status=@StatusPending " +
                        " and expired < current_timestamp at time zone 'UTC'; ";

            var addNewConfirmationQuery = "insert into email_confirmation (customer_id, email, code, expired, attempts, status) " +
                    "select @CustomerId, @Email, @Code, @Expired, @Attempts, @StatusPending " +
                    "where not exists " +
                    "    ( " +
                    "        select null " +
                    "        from email_confirmation ec1 " +
                    "        where ec1.email = @email " +
                    "            and ec1.status = @StatusPending " +
                    "            and ec1.expired > current_timestamp at time zone 'UTC' " +
                    "    ) " +
                    "returning confirmation_id; ";

            return await Connection.TransactionallyAsync(async transaction => 
            {
                await Connection.ExecuteAsync(rejectActiveConfirmationsQuery, parameters, transaction);
                return await Connection.QuerySingleOrDefaultAsync<Guid?>(addNewConfirmationQuery, parameters, transaction);
            });
        }

        public async Task<EmailConfirmation> GetLastConfirmation(string email)
        {
            var query = "select ec.confirmation_id as ConfirmationId, " +
                        "     , ec.customer_id as CustomerId " +
                        "     , ec.email " + 
                        "     , ec.code " +
                        "     , ec.expired " +
                        "     , ec.attempts " +
                        "     , ec.status " +
                        "     , ec.create_timestamp as CreateTimestamp " +
                        "     , ec.update_timestamp as UpdateTimestamp " +
                        "from email_confirmation ec " +
                        "where ec.email = @email " +
                        "    and not exists  " +
                        "    ( " +
                        "        select null " +
                        "        from email_confirmation ec1 " +
                        "        where ec1.email = pc.email " +
                        "            and ec1.create_timestamp > ec.create_timestamp " +
                        "    ); ";

            return await Connection.QuerySingleOrDefaultAsync<EmailConfirmation>(query, new { email });
        }

        public async Task<EmailConfirmation> GetConfirmation(Guid confirmationId)
        {
            var query = "select ec.confirmation_id as ConfirmationId " +
                        "     , ec.customer_id as CustomerId " +
                        "     , ec.email " + 
                        "     , ec.code " +
                        "     , ec.expired " +
                        "     , ec.attempts " +
                        "     , ec.status " +
                        "     , ec.create_timestamp as CreateTimestamp " +
                        "     , ec.update_timestamp as UpdateTimestamp " +
                        "from email_confirmation ec " +
                        "where ec.confirmation_id = @confirmationId";

            return await Connection.QuerySingleOrDefaultAsync<EmailConfirmation>(query, new { confirmationId });    
        }

        public async Task SetAttempts(Guid confirmationId, int attempts)
        {
            var query = "update email_confirmation " +
                        "set attempts = @attempts, " + 
                        "    update_timestamp = current_timestamp at time zone 'UTC' " +
                        "where confirmation_id = @confirmationId; ";
            
            await Connection.ExecuteAsync(query, new { confirmationId, attempts });
        }

        public async Task SetStatus(Guid confirmationId, EmailConfirmationStatus status)
        {
            var query = "update email_confirmation " +
                        "set status = @status, " + 
                        "    update_timestamp = current_timestamp at time zone 'UTC' " +
                        "where confirmation_id = @confirmationId; ";
            
            await Connection.ExecuteAsync(query, new { confirmationId, status });
        }
    }
}