using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Domain;
using TesteBackendEnContact.Core.Interface;
using TesteBackendEnContact.Database;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Repository
{
    public class UserRepository /*: *//*IUserRepository*/
    {
        //private static DatabaseConfig databaseConfig; 

        //public UserRepository(DatabaseConfig databaseConfig)
        //{
        //    this.databaseConfig = databaseConfig;
        //}

        public static async Task<bool> IsValidUser(string login, string keyCode)
        {
            var connection = new SqliteConnection("Data Source=Database.db");
            var query = $"SELECT * FROM USER WHERE Login = '{login}' and KeyCode = '{keyCode}'";
            var result = await connection.QuerySingleOrDefaultAsync<UserDao>(query);
            return result != null;
        }
    }


    [Table("User")]
    public class UserDao : IUser
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string KeyCode { get; set; }

        public UserDao() { }

        public UserDao(IUser user)
        {
            Id = user.Id;
            Login = user.Login;
            KeyCode = user.KeyCode;
        }


        public IUser Export() => new User(Id, Login, KeyCode);
    }
}
