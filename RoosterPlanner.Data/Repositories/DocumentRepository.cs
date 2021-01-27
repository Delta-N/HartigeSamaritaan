using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IDocumentRepository : IRepository<Document>
    {
        /// <summary>
        /// Get the privacy policy document.
        /// </summary>
        /// <returns>A task of the privacy policy document.</returns>
        Task<Document> GetPPAsync();
    }

    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        //constructor
        public DocumentRepository(DbContext dataContext) : base(dataContext)
        {
        }

        /// <summary>
        /// Get the privacy policy document.
        /// </summary>
        /// <returns>A task of the privacy policy document.</returns>
        public Task<Document> GetPPAsync()
        {
            return EntitySet.AsNoTracking()
                .Where(d => d.Name == "Privacy Policy")
                .FirstOrDefaultAsync();
        }
    }
}