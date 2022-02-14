using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface IUserRepository
    {
        Task<bool> IsValidUser(string login, string keyCode);
    }
}
