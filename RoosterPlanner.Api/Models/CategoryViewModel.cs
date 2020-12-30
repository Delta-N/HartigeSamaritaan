using System;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class CategoryViewModel :EntityViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string UrlPdf { get; set; }

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