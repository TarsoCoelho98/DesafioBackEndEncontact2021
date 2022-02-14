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
using TesteBackendEnContact.Core.Interface.ContactBook.Company.Contact;
using TesteBackendEnContact.Core.Domain.ContactBook.Company.Contact;
using System.Collections.Generic;
using System.Text;
//using System.Text;

namespace TesteBackendEnContact.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public ContactRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<IContact> SaveAsync(IContact contact)
        {
            var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var dao = new ContactDao(contact);
            var obj = await GetAsync(contact.Id);

            if (obj != null)
                await connection.UpdateAsync(dao);
            else
                dao.Id = await connection.InsertAsync(dao);

            return dao.Export();
        }

        public async Task DeleteAsync(int id)
        {
            var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var sql = $"DELETE FROM Contact WHERE Id = {id}";
            await connection.ExecuteAsync(sql);
        }

        public async Task<IEnumerable<IContact>> GetAllAsync()
        {
            var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var query = "SELECT * FROM Contact";
            var result = await connection.QueryAsync<ContactDao>(query);
            return result?.Select(item => item.Export());
        }

        public async Task<IContact> GetAsync(int id)
        {
            var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var query = "SELECT * FROM Contact where Id = @id";
            var result = await connection.QuerySingleOrDefaultAsync<ContactDao>(query, new { id });
            return result;
        }


        [Table("Contact")]
        public class ContactDao : IContact
        {
            [Key]
            public int Id { get; set; }
            public int ContactBookId { get; set; }
            public int? CompanyId { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Adress { get; set; }

            public ContactDao() { }

            public ContactDao(IContact contact)
            {
                Id = contact.Id;
                ContactBookId = contact.ContactBookId;
                CompanyId = contact.CompanyId;
                Name = contact.Name;
                Phone = contact.Phone;
                Email = contact.Email;
                Adress = contact.Adress;
            }

            public IContact Export() => new Contact(Id, ContactBookId, CompanyId, Name, Phone, Email, Adress);
        }
    }
}
