using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain.ContactBook.Company;
using TesteBackendEnContact.Core.Interface.ContactBook.Company;
using TesteBackendEnContact.Database;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public CompanyRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<ICompany> SaveAsync(ICompany company)
        {
            var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var dao = new CompanyDao(company);
            var obj = await GetAsync(company.Id);

            if (obj != null)
                await connection.UpdateAsync(dao);
            else
                dao.Id = await connection.InsertAsync(dao);

            return dao.Export();
        }

        public async Task DeleteAsync(int id)
        {
            var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var sql = $"UPDATE Contact SET CompanyId = null WHERE CompanyId = @id";
            await connection.ExecuteAsync(sql, new { id });
            sql = $"DELETE FROM Company WHERE Id = @id";
            await connection.ExecuteAsync(sql, new { id });
        }

        public async Task<IEnumerable<ICompany>> GetAllAsync()
        {
            var connection = new SqliteConnection(databaseConfig.ConnectionString);

            var query = "SELECT * FROM Company";
            var result = await connection.QueryAsync<CompanyDao>(query);

            return result?.Select(item => item.Export());
        }

        public async Task<ICompany> GetAsync(int id)
        {
            var connection = new SqliteConnection(databaseConfig.ConnectionString);
            var query = "SELECT * FROM Company where Id = @id";
            var result = await connection.QuerySingleOrDefaultAsync<CompanyDao>(query, new { id });
            return result;
        }
    }

    [Table("Company")]
    public class CompanyDao : ICompany
    {
        [Key]
        public int Id { get; set; }
        public int ContactBookId { get; set; }
        public string Name { get; set; }

        public CompanyDao() { }

        public CompanyDao(ICompany company)
        {
            Id = company.Id;
            ContactBookId = company.ContactBookId;
            Name = company.Name;
        }


        public ICompany Export() => new Company(Id, ContactBookId, Name);
    }
}
