using FoodApp.Api.Data.Entities;

namespace FoodApp.Api.Repository.Specification.UsesrSpec
{
    public class CountUserWithSpec : BaseSpecification<User>
    {
        public CountUserWithSpec(SpecParams specParams)
           : base(p => !p.IsDeleted)
        {

        }
    }
}
