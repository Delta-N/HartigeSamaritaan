using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IDocumentRepository : IRepository<Document>
    {
        Task<Document> GetPPAsync();
    }

    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        //constructor
        public DocumentRepository(DbContext dataContext) : base(dataContext)
        {
        }

        public Task<Document> GetPPAsync()
        {
            return EntitySet.AsNoTracking()
                .Where(d => d.Name == "Privacy Policy")
                .FirstOrDefaultAsync();
        }
    }
}