using FBDropshipper.Application.Extensions;
using FBDropshipper.Persistence.Context;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FBDropshipper.Application.CatalogProducts.Queries.CheckIfCatalogProductExistBySku;

public class CheckIfCatalogProductExistBySkuRequestModel : IRequest<CheckIfCatalogProductExistBySkuResponseModel>
{
    public string SkuCode { get; set; }
}

public class
    CheckIfCatalogProductExistBySkuRequestModelValidator : AbstractValidator<
        CheckIfCatalogProductExistBySkuRequestModel>
{
    public CheckIfCatalogProductExistBySkuRequestModelValidator()
    {
        RuleFor(p => p.SkuCode).Required();
    }
}

public class
    CheckIfCatalogProductExistBySkuRequestHandler : IRequestHandler<CheckIfCatalogProductExistBySkuRequestModel,
        CheckIfCatalogProductExistBySkuResponseModel>
{
    private readonly ApplicationDbContext _context;

    public CheckIfCatalogProductExistBySkuRequestHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CheckIfCatalogProductExistBySkuResponseModel> Handle(
        CheckIfCatalogProductExistBySkuRequestModel request,
        CancellationToken cancellationToken)
    {
        var result = await _context.CatalogProducts.AnyAsync(pr => pr.SkuCode == request.SkuCode, cancellationToken: cancellationToken);
        return new CheckIfCatalogProductExistBySkuResponseModel()
        {
            Exists = result
        };
    }

}

public class CheckIfCatalogProductExistBySkuResponseModel
{
    public bool Exists { get; set; }
}