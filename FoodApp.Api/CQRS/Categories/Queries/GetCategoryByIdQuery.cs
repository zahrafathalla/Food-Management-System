using FoodApp.Api.Abstraction;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using FoodApp.Api.Repository.Specification.CategorySpec;
using FoodApp.Api.Response;
using MediatR;

namespace FoodApp.Api.CQRS.Categories.Queries
{
    public record GetCategoryByIdQuery(int CategoryId) : IRequest<Result<Category>>;

    public record CategoryToReturnDto(int Id, string Name, List<RecipesNamesToReturnDto> Recipes);
    public record RecipesNamesToReturnDto(int Id ,string Name);

    public class GetCategoryByIdQueryHandler : BaseRequestHandler<GetCategoryByIdQuery, Result<Category>>
    {
        public GetCategoryByIdQueryHandler(RequestParameters requestParameters) : base(requestParameters) { }

        public override async Task<Result<Category>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new CategoryWithRecipesSpecification(request.CategoryId);
            var category = await _unitOfWork.Repository<Category>().GetByIdWithSpecAsync(spec);
            if (category == null)
            {
                return Result.Failure<Category>(CategoryErrors.CategoryNotFound);
            }


            return Result.Success(category);
        }
    }
}
