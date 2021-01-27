using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class CategoryViewModel : EntityViewModel
    {
        /// <summary>
        /// Gets or sets the Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Creates a ViewModel from a Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static CategoryViewModel CreateVm(Category category)
        {
            if (category != null)
            {
                return new CategoryViewModel
                {
                    Id = category.Id,
                    Code = category.Code,
                    Name = category.Name,
                    LastEditDate = category.LastEditDate,
                    LastEditBy = category.LastEditBy,
                    RowVersion = category.RowVersion
                };
            }

            return null;
        }

        /// <summary>
        /// Creates a Category from a ViewModel
        /// </summary>
        /// <param name="categoryViewModel"></param>
        /// <returns></returns>
        public static Category CreateCategory(CategoryViewModel categoryViewModel)
        {
            if (categoryViewModel != null)
            {
                return new Category(categoryViewModel.Id)
                {
                    Code = categoryViewModel.Code,
                    Name = categoryViewModel.Name,
                    LastEditDate = categoryViewModel.LastEditDate,
                    LastEditBy = categoryViewModel.LastEditBy,
                    RowVersion = categoryViewModel.RowVersion
                };
            }

            return null;
        }
    }
}