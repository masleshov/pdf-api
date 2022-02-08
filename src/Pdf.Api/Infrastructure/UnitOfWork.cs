using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using Npgsql;
using Pdf.Api.Configuration;
using Pdf.Api.Repository;

namespace Pdf.Api.Infrastructure
{
    internal sealed class UnitOfWork : IDisposable
    {
        private readonly NpgsqlConnection _connection;
        private readonly Dictionary<Type, BaseRepository> _repositories;

        public UnitOfWork(IOptions<NpgsqlDatabaseConfiguration> configuration)
        {
            var connectionString = configuration.Value.Builder.ConnectionString;
            if(string.IsNullOrEmpty(connectionString))
            {
                throw new NullReferenceException("Connection string to database must be not null");
            }

            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            _repositories = GetRepositoriesByReflection();
        }

        /// <summary>
        /// Returns a repository by specified type
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Throws if repository hasn't been registered and can't be found</exception>
        public TRepository GetRepository<TRepository>() where TRepository : BaseRepository
        {
            if(!_repositories.TryGetValue(typeof(TRepository), out var repository))
            {
                throw new InvalidOperationException($"Repository of type {typeof(TRepository).Name} doesn't exist");
            }

            return (TRepository)repository;
        }

        private Dictionary<Type, BaseRepository> GetRepositoriesByReflection()
        {
            return Assembly.GetExecutingAssembly().DefinedTypes
                .Where(type => type.IsSubclassOf(typeof(BaseRepository)))
                .ToDictionary(key => key.AsType(), value => (BaseRepository)Activator.CreateInstance(value.AsType(), _connection));
        }

        public void Dispose()
        {
            _repositories.Clear();
            _connection.Dispose();
        }
    }
}