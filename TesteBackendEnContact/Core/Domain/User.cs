using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interface;

namespace TesteBackendEnContact.Core.Domain
{
    public class User : IUser
    {
        public int Id { get; private set; }
        public string Login { get; private set; }
        public string KeyCode { get; private set; }

        public User(int id, string login, string keyCode)
        {
            Id = id;
            Login = login;
            KeyCode = keyCode;
        }
    }
}
