using FBDropshipper.Application.Extensions;
using FBDropshipper.Persistence.Context;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.InventoryProductImages.Commands.FixInventoryProductImageOrder;

public class FixInventoryProductImageOrderRequestModel : IRequest<FixInventoryProductImageOrderResponseModel>
{
    public int InventoryProductId { get; set; }
}

public class
    FixInventoryProductImageOrderRequestModelValidator : AbstractValidator<FixInventoryProductImageOrderRequestModel>
{
    public FixInventoryProductImageOrderRequestModelValidator()
    {
        RuleFor(p => p.InventoryProductId).Required();
    }
}

public class
    FixInventoryProductImageOrderRequestHandler : IRequestHandler<FixInventoryProductImageOrderRequestModel,
        FixInventoryProductImageOrderResponseModel>
{
    private readonly ApplicationDbContext _context;

    public FixInventoryProductImageOrderRequestHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FixInventoryProductImageOrderResponseModel> Handle(FixInventoryProductImageOrderRequestModel request,
        CancellationToken cancellationToken)
    {
        var images = _context.InventoryProductImages.Where(p => p.InventoryProductId == request.InventoryProductId)
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
        _context.InventoryProductImages.UpdateRange(images);
        await _context.SaveChangesAsync(cancellationToken);
        return new FixInventoryProductImageOrderResponseModel();
    }

}

public class FixInventoryProductImageOrderResponseModel
{

}