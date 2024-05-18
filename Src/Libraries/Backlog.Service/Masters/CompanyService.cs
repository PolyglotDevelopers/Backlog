using Backlog.Core.Domain.Masters;
using Backlog.Data.Repository;

namespace Backlog.Service.Masters
{
    public class CompanyService : ICompanyService
    {
        #region Fields

        protected readonly IRepository<Company> _companyRepository;

        #endregion

        #region Ctor
        public CompanyService(IRepository<Company> companyRepository)
        {
            _companyRepository = companyRepository;
        }
        #endregion

        #region Methods

        public async Task<Company> GetByIdAsync(int id)
        {
            return id == 0 ? null : await _companyRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Company entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _companyRepository.UpdateAsync(entity);
        }

        #endregion
    }
}
