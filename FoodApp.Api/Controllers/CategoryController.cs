using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Categories.Commands;
using FoodApp.Api.CQRS.Categories.Queries;
using FoodApp.Api.CQRS.Recipes.Commands;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using FoodApp.Api.Response;
using FoodApp.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Helper;

namespace FoodApp.Api.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(ControllerParameters controllerParameters) : base(controllerParameters) { }


        [HttpPost("AddCategory")]
        public async Task<Result<int>> AddCateory([FromForm] CreateCategoryViewModel viewModel)
        {
            var command = viewModel.Map<CreateCategoryCommand>();
            var response = await _mediator.Send(command);
            return response;
        }

        [HttpPut("UpdateCategory/{categoryId}")]
        public async Task<Result<bool>> UpdateCategory(int categoryId, [FromBody] CreateCategoryViewModel viewModel)
        {
            var command = new UpdateCategoryCommand(categoryId, viewModel.Name);
            var response = await _mediator.Send(command);
            return response;
        }


        [HttpGet("ViewCategory/{categoryId}")]
        public async Task<Result<CategoryToReturnDto>> GetCategoryById(int categoryId)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(categoryId));
            if(!result.IsSuccess)
            {
                return Result.Failure<CategoryToReturnDto>(CategoryErrors.CategoryNotFound);
            }
            var category =result.Data.Map<CategoryToReturnDto>();
            return Result.Success(category);
        }


        [HttpDelete("DeleteCategory/{categoryId}")]
        public async Task<Result<bool>> DeleteCategory(int categoryId)
        {
            var command = new DeleteCategoryCommand(categoryId);
            var response = await _mediator.Send(command);
            return response;
        }

    }
}
