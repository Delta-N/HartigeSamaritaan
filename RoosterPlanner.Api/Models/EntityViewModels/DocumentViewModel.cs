using RoosterPlanner.Models.Models;
namespace RoosterPlanner.Api.Models.EntityViewModels
{
    public class DocumentViewModel : EntityViewModel
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the DocumentUri 
        /// </summary>
        public string DocumentUri { get; set; }

        /// <summary>
        /// Creates a ViewModel from a document.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a Document from a ViewModel.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
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