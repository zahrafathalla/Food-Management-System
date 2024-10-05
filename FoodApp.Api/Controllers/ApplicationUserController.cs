﻿using FoodApp.Api.Abstraction;
using FoodApp.Api.CQRS.Discounts.Commands;
using FoodApp.Api.CQRS.Users.Commands;
using FoodApp.Api.CQRS.Users.Queries;
using FoodApp.Api.DTOs;
using FoodApp.Api.Helper;
using FoodApp.Api.Repository.Specification;
using FoodApp.Api.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Helper;

namespace FoodApp.Api.Controllers
{

    public class ApplicationUserController : BaseController
    {
        public ApplicationUserController(ControllerParameters controllerParameters) : base(controllerParameters) { }

        [HttpGet("GetAllUsers")]
        public async Task<Result<Pagination<UserToReturnDto>>> GetAllUsers([FromQuery] SpecParams spec)
        {
            var result = await _mediator.Send(new GetAllUsersQuery(spec));
            if (!result.IsSuccess)
            {
                return Result.Failure<Pagination<UserToReturnDto>>(result.Error);
            }

            var UsertCount = await _mediator.Send(new GetUserCountQuery(spec));
            var paginationResult = new Pagination<UserToReturnDto>(spec.PageSize, spec.PageIndex, UsertCount.Data, result.Data);
            return Result.Success(paginationResult);
        }


        [HttpGet("GetUserProfile")]
        public async Task<Result<UserToReturnDto>> GetUserProfile()
        {
            var command = new GetUserProfileQuery();
            var result =await _mediator.Send(command);
            return result;
        }

        [HttpPut("UpdateUserProfile")]
        public async Task<Result<bool>> UpdateUser(UpdateUserProfileViewModel viewModel)
        {
            var command = viewModel.Map<UpdateUserProfileCommand>();
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpDelete("DeleteUserProfile")]
        public async Task<Result<bool>> DeleteUser()
        {
            var result = await _mediator.Send(new DeleteUserProfileCommand());
            return result;
        }

        [HttpPost("ChangeUserPassword")]
        public async Task<Result<bool>> ChangePassword(ChangePasswordViewModel viewModel)
        {
            var command = viewModel.Map<ChangePasswordCommand>();
            var response = await _mediator.Send(command);
            return response;
        }

    }
}
