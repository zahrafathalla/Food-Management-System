﻿using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Categories.Queries;
using FoodApp.Api.Data.Entities;
using FoodApp.Api.DTOs;
using FoodApp.Api.Errors;
using MediatR;
using ProjectManagementSystem.Helper;

namespace FoodApp.Api.CQRS.Categories.Commands
{
    public record DeleteCategoryCommand(int CategoryId) : IRequest<Result<bool>>;
    public class DeleteCategoryCommandHandler : BaseRequestHandler<DeleteCategoryCommand, Result<bool>>
    {
        public DeleteCategoryCommandHandler(RequestParameters requestParameters) : base(requestParameters) { }

        public override async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        { 
            var categoryResult = await _mediator.Send(new GetCategoryByIdQuery(request.CategoryId), cancellationToken);
            if (!categoryResult.IsSuccess)
            {
                return Result.Failure<bool>(CategoryErrors.CategoryNotFound);
            }

            var category = categoryResult.Data;

            _unitOfWork.Repository<Category>().Delete(category);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }
}
