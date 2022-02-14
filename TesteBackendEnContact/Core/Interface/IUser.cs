using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TesteBackendEnContact.Core.Interface
{
    public interface IUser
    {
        int Id { get; }
        string Login { get; }
        string KeyCode { get; }
    }
}
