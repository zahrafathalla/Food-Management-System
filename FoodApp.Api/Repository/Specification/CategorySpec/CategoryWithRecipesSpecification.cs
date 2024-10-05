using FoodApp.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodApp.Api.Repository.Specification.CategorySpec
{
    public class CategoryWithRecipesSpecification :BaseSpecification<Category>
    {
        public CategoryWithRecipesSpecification(int id)
            :base(c => c.Id == id)
        {
            Includes.Add(c=>c.Include(c=>c.Recipes));
        }
    }
}
