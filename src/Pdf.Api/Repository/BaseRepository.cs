using Npgsql;

namespace Pdf.Api.Repository
{
    internal abstract class BaseRepository
    {
        protected readonly NpgsqlConnection Connection;

        public BaseRepository(NpgsqlConnection connection) => Connection = connection;
    }
}