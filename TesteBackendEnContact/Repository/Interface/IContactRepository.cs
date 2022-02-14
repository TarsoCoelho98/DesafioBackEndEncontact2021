using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesteBackendEnContact.Core.Interface.ContactBook.Company.Contact;

namespace TesteBackendEnContact.Repository.Interface
{
    public interface IContactRepository
    {
        Task<IContact> SaveAsync(IContact contactBook);
        Task DeleteAsync(int id);
        Task<IEnumerable<IContact>> GetAllAsync();
        Task<IContact> GetAsync(int id);
    }
}
