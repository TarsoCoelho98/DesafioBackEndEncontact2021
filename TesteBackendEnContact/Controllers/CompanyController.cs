using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteBackendEnContact.Controllers.Models;
using TesteBackendEnContact.Core.Interface.ContactBook.Company;
using TesteBackendEnContact.Repository.Interface;

namespace TesteBackendEnContact.Controllers
{
    [ApiController]
    [
        ("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ILogger<CompanyController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ICompany> Post(SaveCompanyRequest company, [FromServices] ICompanyRepository companyRepository)
        {
            return await companyRepository.SaveAsync(company.ToCompany());
        }

        [HttpDelete]
        public async Task Delete(int id, [FromServices] ICompanyRepository companyRepository)
        {
            await companyRepository.DeleteAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<ICompany>> Get([FromServices] ICompanyRepository companyRepository)
        {
            return await companyRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ICompany> Get(int id, [FromServices] ICompanyRepository companyRepository)
        {
            return await companyRepository.GetAsync(id);
        }
    }
}
