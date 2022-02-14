using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.ContactBook;
using TesteBackendEnContact.Core.Interface.ContactBook;
using TesteBackendEnContact.Database;
using TesteBackendEnContact.Repository.Interface;
using System.Linq;
using System;

namespace TesteBackendEnContact.Repository
{
    public class ContactBookRepository : IContactBookRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public ContactBookRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<IContactBook> SaveAsync(IContactBook contactBook)
        {
            var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var dao = new ContactBookDao(contactBook);
            var obj = await GetAsync(contactBook.Id);

            if (obj == null)
                dao.Id = await connection.InsertAsync(dao);

            return dao.Export();
        }

        public async Task DeleteAsync(int id)
        {
            var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var sql = $"DELETE FROM ContactBook WHERE Id = {id}";
            await connection.ExecuteAsync(sql);

            sql = $"DELETE FROM Company WHERE ContactBookId = {id}";
            await connection.ExecuteAsync(sql, new { id });

            sql = $"DELETE FROM Contact WHERE ContactBookId = {id}";
            await connection.ExecuteAsync(sql, new { id });
        }

        public async Task<IEnumerable<IContactBook>> GetAllAsync()
        {
            var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var query = "SELECT * FROM ContactBook";
            var result = await connection.QueryAsync<ContactBookDao>(query);

            return result?.Select(item => item.Export());
        }

        public async Task<IContactBook> GetAsync(int id)
        {
            var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var query = $"SELECT * FROM ContactBook WHERE Id = {id}";
            var obj = await connection.QueryFirstOrDefaultAsync<ContactBookDao>(query);
            return obj;
        }
    }

    [Table("ContactBook")]
    public class ContactBookDao : IContactBook
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public ContactBookDao() { }
        public ContactBookDao(IContactBook contactBook)
        {
            Id = contactBook.Id;
            Name = contactBook.Name;
        }
        public IContactBook Export() => new ContactBook(Id, Name);
    }
}
