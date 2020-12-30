using Microsoft.EntityFrameworkCore;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models;

namespace RoosterPlanner.Data.Repositories
{
    public interface IDocumentRepository : IRepository<Document>
    {
    }

    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        //constructor
        public DocumentRepository(DbContext dataContext) : base(dataContext)
        {
        }
        
    }
}