using System;
using System.Threading.Tasks;
using Npgsql;

namespace Pdf.Api.Extension
{
    internal static class NpgsqlConnectionExtension
    {
        public static async Task<TResult> TransactionallyAsync<TResult>(
            this NpgsqlConnection connection, 
            Func<NpgsqlTransaction, Task<TResult>> queries
        )
        {
            using (var transaction = connection.BeginTransaction())
            {
                var result = await queries(transaction);
                transaction.Commit();
                return result;
            }
        }

        public static async Task TransactionallyAsync(this NpgsqlConnection connection, Func<NpgsqlTransaction, Task> queries)
        {
            using (var transaction = connection.BeginTransaction())
            {
                await queries(transaction);
                transaction.Commit();
            }
        }
    }
}