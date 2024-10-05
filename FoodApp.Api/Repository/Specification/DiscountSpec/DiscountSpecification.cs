using FoodApp.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodApp.Api.Repository.Specification.DiscountSpec
{
    public class DiscountSpecification : BaseSpecification<Discount>
    {
        public DiscountSpecification(int id)
           : base(d=>d.Id == id)
        {
            Includes.Add(d=>d.Include(d=>d.RecipeDiscounts));
        }
    }
}
