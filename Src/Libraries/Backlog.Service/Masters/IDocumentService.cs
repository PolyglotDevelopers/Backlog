using Microsoft.AspNetCore.Http;
using Backlog.Core.Domain.Masters;

namespace Backlog.Service.Masters
{
    public interface IDocumentService
    {
        Task<Document> GetByIdAsync(int id);

        Task<Document> InsertAndGetAsync(IFormFile file, int employeeId);

        Task UpdateAsync(int documentId, IFormFile file, int employeeId);

        Task DeleteAsync(Document entity);
    }
}
