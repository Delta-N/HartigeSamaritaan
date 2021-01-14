using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class DocumentViewModel : EntityViewModel
    {
        public string Name { get; set; }

        public string DocumentUri { get; set; }

        public static DocumentViewModel CreateVm(Document document)
        {
            if (document != null)
            {
                return new DocumentViewModel
                {
                    Id = document.Id,
                    Name = document.Name,
                    DocumentUri = document.DocumentUri,
                    LastEditDate = document.LastEditDate,
                    LastEditBy = document.LastEditBy,
                    RowVersion = document.RowVersion
                };
            }

            return null;
        }

        public static Document CreateDocument(DocumentViewModel viewModel)
        {
            if (viewModel != null)
            {
                return new Document(viewModel.Id)
                {
                    Name = viewModel.Name,
                    DocumentUri = viewModel.DocumentUri,
                    LastEditDate = viewModel.LastEditDate,
                    LastEditBy = viewModel.LastEditBy,
                    RowVersion = viewModel.RowVersion
                };
            }
            return null;
        }
    }
}