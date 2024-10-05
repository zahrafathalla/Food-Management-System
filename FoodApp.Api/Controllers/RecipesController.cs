﻿using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Categories.Queries;
using FoodApp.Api.CQRS.FavouriteRecipes.Commands;
using FoodApp.Api.CQRS.FavouriteRecipes.Queries;
using FoodApp.Api.CQRS.Recipes.Commands;
using FoodApp.Api.CQRS.Recipes.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using FoodApp.Api.Helper;
using FoodApp.Api.Repository.Specification;
using FoodApp.Api.Response;
using FoodApp.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Helper;

namespace FoodApp.Api.Controllers;

public class RecipesController : BaseController
{
    public RecipesController(ControllerParameters controllerParameters) : base(controllerParameters) { }

    //[Authorize]
    [HttpPost("AddRecipe")]
    public async Task<Result<bool>> AddRecipe([FromForm] CreateRecipeViewModel viewModel)
    {
        var command = viewModel.Map<CreateRecipeCommand>();
        var response = await _mediator.Send(command);
        return response;
    }

    //[Authorize]
    [HttpPut("UpdateRecipe")]
    public async Task<Result<bool>> UpdateRecipe([FromForm] UpdateRecipeViewModel viewModel)
    {
        var command = viewModel.Map<UpdateRecipeCommand>();
        var response = await _mediator.Send(command);
        return response;
    }

    //[Authorize]
    [HttpDelete("DeleteRecipe")]
    public async Task<Result<bool>> DeleteRecipe(int RecipeId)
    {
        var command = new DeleteRecipeCommand(RecipeId);
        var response = await _mediator.Send(command);
        return response;
    }

    //[Authorize]
    [HttpGet("ViewRecipe/{RecipeId}")]
    public async Task<Result<RecipeToReturnDto>> GetRecipeById(int RecipeId)
    {
        var command = new GetRecipeByIdQuery(RecipeId);
        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
        {
            return Result.Failure<RecipeToReturnDto>(RecipeErrors.RecipeNotFound);
        }
        var recipe = result.Data;

        var response = recipe.Map<RecipeToReturnDto>();

        return Result.Success(response);
    }

    //[Authorize]
    [HttpGet("ListRecipes")]
    public async Task<Result<Pagination<RecipeToReturnDto>>> GetAllRecipes([FromQuery] SpecParams spec)
    {
        var result = await _mediator.Send(new GetRecipesQuery(spec));
        if (!result.IsSuccess)
        {
            return Result.Failure<Pagination<RecipeToReturnDto>>(result.Error);
        }

        var RecipesCount = await _mediator.Send(new GetRecipesCountQuery(spec));
        var paginationResult = new Pagination<RecipeToReturnDto>(spec.PageSize, spec.PageIndex, RecipesCount.Data, result.Data);

        return Result.Success(paginationResult);
    }
    [HttpPost("AddRecipeToFavourite")]
    public async Task<Result<bool>> AddRecipeToFavourite([FromForm] AddRecipeToFavouritesViewModel viewModel)
    {
        var command = viewModel.Map<AddRecipeToFavouritesCommand>();
        var response = await _mediator.Send(command);
        return response;
    }
    [HttpDelete("RemoveRecipeFromFavourite/{RecipeId}")]
    public async Task<Result<bool>> RemoveRecipeFromFavourite(int RecipeId)
    {
        var response = await _mediator.Send(new RemoveRecipeFromFavouritesCommand(RecipeId));
        return response;
    }
    [HttpGet("ViewFavouriteRecipes")]
    public async Task<IActionResult> ViewFavouriteRecipes()
    {
        var result = await _mediator.Send(new GetFavouriteRecipesForLoggedUserQuery());

        if (!result.IsSuccess)
        {
            return NotFound(result.Error);
        }

        return Ok(Result.Success(result.Data));
    }

}
