using FBDropshipper.Domain.Constant;
using FBDropshipper.Persistence.Context;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.TeamMembers.Queries.GetPermissions;

public class GetPermissionsRequestModel : IRequest<GetPermissionsResponseModel>
{

}

public class GetPermissionsRequestModelValidator : AbstractValidator<GetPermissionsRequestModel>
{
    public GetPermissionsRequestModelValidator()
    {

    }
}

public class
    GetPermissionsRequestHandler : IRequestHandler<GetPermissionsRequestModel, GetPermissionsResponseModel>
{

    public Task<GetPermissionsResponseModel> Handle(GetPermissionsRequestModel request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new GetPermissionsResponseModel()
        {
            Permissions = AppPolicy.BuildAllPermissions()
        });
    }

}

public class GetPermissionsResponseModel
{
    public string[] Permissions { get; set; }
}