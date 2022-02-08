using Npgsql;

namespace Pdf.Api.Configuration
{
    // https://www.npgsql.org/doc/connection-string-parameters.html
    public record NpgsqlDatabaseConfiguration
    {
        public string Host { get; init; }
        public int Port { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string Database { get; init; }
        public string ApplicationName { get; init; }
        public int MinPoolSize { get; init; }
        public int MaxPoolSize { get; init; }

        public NpgsqlConnectionStringBuilder Builder {
            get => new()
            {
                Host = Host,
                Port = Port,
                Username = Username,
                Password = Password,
                Database = Database,
                ApplicationName = ApplicationName,
                MinPoolSize = MinPoolSize,
                MaxPoolSize = MaxPoolSize,
            };
        }
    }
}