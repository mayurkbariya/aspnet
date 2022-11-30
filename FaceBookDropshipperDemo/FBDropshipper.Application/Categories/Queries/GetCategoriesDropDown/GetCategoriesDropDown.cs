using FBDropshipper.Application.Categories.Models;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.Categories.Queries.GetCategoriesDropDown;

public class GetCategoriesDropDownRequestModel : IRequest<GetCategoriesDropDownResponseModel>
{

}

public class GetCategoriesDropDownRequestModelValidator : AbstractValidator<GetCategoriesDropDownRequestModel>
{
    public GetCategoriesDropDownRequestModelValidator()
    {

    }
}

public class
    GetCategoriesDropDownRequestHandler : IRequestHandler<GetCategoriesDropDownRequestModel,
        GetCategoriesDropDownResponseModel>
{
    private readonly ApplicationDbContext _context;

    public GetCategoriesDropDownRequestHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetCategoriesDropDownResponseModel> Handle(GetCategoriesDropDownRequestModel request,
        CancellationToken cancellationToken)
    {
        var list = await _context.Categories.GetAllReadOnly()
            .Select(CategorySelector.SelectorDropDown)
            .ToListAsync(cancellationToken: cancellationToken);
        return new GetCategoriesDropDownResponseModel()
        {
            Data = list
        };
    }

}

public class GetCategoriesDropDownResponseModel
{
    public List<CategoryDropDownDto> Data { get; set; }
}