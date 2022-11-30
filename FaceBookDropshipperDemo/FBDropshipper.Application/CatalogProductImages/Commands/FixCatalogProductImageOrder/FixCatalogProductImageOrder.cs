using FBDropshipper.Application.Extensions;
using FBDropshipper.Persistence.Context;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.CatalogProductImages.Commands.FixCatalogProductImageOrder;

public class FixCatalogProductImageOrderRequestModel : IRequest<FixCatalogProductImageOrderResponseModel>
{
    public int CatalogProductId { get; set; }
}

public class
    FixCatalogProductImageOrderRequestModelValidator : AbstractValidator<FixCatalogProductImageOrderRequestModel>
{
    public FixCatalogProductImageOrderRequestModelValidator()
    {
        RuleFor(p => p.CatalogProductId).Required();
    }
}

public class
    FixCatalogProductImageOrderRequestHandler : IRequestHandler<FixCatalogProductImageOrderRequestModel,
        FixCatalogProductImageOrderResponseModel>
{
    private readonly ApplicationDbContext _context;

    public FixCatalogProductImageOrderRequestHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FixCatalogProductImageOrderResponseModel> Handle(FixCatalogProductImageOrderRequestModel request,
        CancellationToken cancellationToken)
    {
        var images = _context.CatalogProductImages.Where(p => p.CatalogProductId == request.CatalogProductId)
            .ToList();
        var order = 1;
        images = images.OrderBy(p => p.Order).
            ThenByDescending(p => p.UpdatedDate)
            .ToList();
        foreach (var image in images)
        {
            if (image.Order == order)
            {
                order++;
                continue;
            }
            image.Order = order;
            order++;
        }
        _context.CatalogProductImages.UpdateRange(images);
        await _context.SaveChangesAsync(cancellationToken);
        return new FixCatalogProductImageOrderResponseModel();
    }

}

public class FixCatalogProductImageOrderResponseModel
{

}