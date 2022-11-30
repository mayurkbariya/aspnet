using FBDropshipper.Application.Extensions;
using FBDropshipper.Persistence.Context;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.ProductListingImages.Commands.FixProductListingImageOrder;

public class FixProductListingImageOrderRequestModel : IRequest<FixProductListingImageOrderResponseModel>
{
    public int Id { get; set; }
}

public class
    FixProductListingImageOrderRequestModelValidator : AbstractValidator<FixProductListingImageOrderRequestModel>
{
    public FixProductListingImageOrderRequestModelValidator()
    {
        RuleFor(p => p.Id).Required();
    }
}

public class
    FixProductListingImageOrderRequestHandler : IRequestHandler<FixProductListingImageOrderRequestModel,
        FixProductListingImageOrderResponseModel>
{
    private readonly ApplicationDbContext _context;

    public FixProductListingImageOrderRequestHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FixProductListingImageOrderResponseModel> Handle(FixProductListingImageOrderRequestModel request,
        CancellationToken cancellationToken)
    {
        var images = _context.ProductListingImages.Where(p => p.ProductListingId == request.Id)
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
        _context.ProductListingImages.UpdateRange(images);
        await _context.SaveChangesAsync(cancellationToken);
        return new FixProductListingImageOrderResponseModel();
    }

}

public class FixProductListingImageOrderResponseModel
{

}