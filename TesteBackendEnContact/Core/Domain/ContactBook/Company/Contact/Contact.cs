using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interface.ContactBook.Company.Contact;

namespace TesteBackendEnContact.Core.Domain.ContactBook.Company.Contact
{
    public class Contact : IContact
    {
        public int Id { get; private set; }
        public int ContactBookId { get; private set; }
        public int ?CompanyId { get; private set; }
        public string Name { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public string Adress { get; private set; }

        public Contact(int id, int contactBookId, int? companyId, string name, string phone, string email, string adress)
        {
            Id = id;
            ContactBookId = contactBookId;
            CompanyId = companyId;
            Name = name;
            Phone = phone;
            Email = email;
            Adress = adress;
        }
    }
}
