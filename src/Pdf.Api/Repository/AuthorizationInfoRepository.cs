using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Pdf.Api.Domain;

namespace Pdf.Api.Repository
{
    internal sealed class AuthorizationInfoRepository : BaseRepository
    {
        public AuthorizationInfoRepository(NpgsqlConnection connection) : base(connection)
        {
        }

        public async Task<AuthorizationInfo> GetAuthorizationInfo(int customerId) 
        {
            var query = "select customer_id as CustomerId " +
                        "     , access_token as AccessToken " +
                        "     , refresh_token as RefreshToken " +
                        "     , expired " +
                        "     , create_timestamp as CreateTimestamp " +
                        "     , update_timestamp as UpdateTimestamp " +
                        "from authorization_info " +
                        "where customer_id = @customerId";

            return await Connection.QuerySingleOrDefaultAsync<AuthorizationInfo>(query, new { customerId });
        }

        public async Task AddAuthorizationInfo(AuthorizationInfo authorizationInfo) 
        {
            var parameters = new {
                CustomerId = authorizationInfo.CustomerId,
                AccessToken = authorizationInfo.AccessToken,
                RefreshToken = authorizationInfo.RefreshToken,
                Expired = authorizationInfo.Expired
            };

            var query = "insert into authorization_info(customer_id, access_token, refresh_token, expired) " +
                        "values (@CustomerId, @AccessToken, @RefreshToken, @Expired)";

            await Connection.ExecuteAsync(query, parameters);
        }

        public async Task UpdateAuthorizationInfo(AuthorizationInfo authorizationInfo) 
        {
            var parameters = new {
                CustomerId = authorizationInfo.CustomerId,
                AccessToken = authorizationInfo.AccessToken,
                RefreshToken = authorizationInfo.RefreshToken,
                Expired = authorizationInfo.Expired
            };

            var query = "update authorization_info " +
                    "set access_token = @AccessToken, " +
                    "    refresh_token = @RefreshToken, " +
                    "    expired = @Expired, " +
                    "    update_timestamp = current_timestamp at time zone 'UTC' " +
                    "where customer_id = @CustomerId";                        

            await Connection.ExecuteAsync(query, parameters);
        }
    }
}