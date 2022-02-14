using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TesteBackendEnContact.Core.Interface.ContactBook.Company.Contact
{
    public interface IContact
    {
        int Id { get; }
        int ContactBookId { get; }
        int? CompanyId { get; }
        string Name { get; }
        string Phone { get; }
        string Email { get; }
        string Adress { get; }
    }
}
