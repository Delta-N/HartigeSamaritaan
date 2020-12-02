using System;
using RoosterPlanner.Models;

namespace RoosterPlanner.Api.Models
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
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
                    UrlPdf = category.UrlPdf
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
                    UrlPdf = categoryViewModel.UrlPdf
                };
            }

            return null;
        }
    }
}