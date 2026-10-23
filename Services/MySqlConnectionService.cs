using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace SQLstudio.Services
{
    public class MySqlConnectionService
    {
        public async Task TestConnAsync(string connectionString, 
            CancellationToken cancellationToken = default)
        {
            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);
        }

        public async Task<string> GetServerVersionAsync(string connectionString, CancellationToken cancellationToken = default)
        {
            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);
            await using var command = new MySqlCommand("SELECT VERSION();", connection);
            var result = await command.ExecuteScalarAsync(cancellationToken);

            if(result is string version)
            {
                return version;
            }
            else
            {
                throw new InvalidOperationException("Valor nao e string");
            }
        }

        public async Task<List<string>> ListDatabasesAsync(string connectionString,
            CancellationToken cancellationToken)
        {
            List<string> databases = new();

            await using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);
            await using var command = new MySqlCommand("SHOW DATABASES;", connection);
            
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while(await reader.ReadAsync(cancellationToken))
            {
                string databaseName = reader.GetString(0);
                databases.Add(databaseName);
            }

            return databases;
        }
    }
}
