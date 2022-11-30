using FBDropshipper.Application.Extensions;
using FBDropshipper.Application.Interfaces;
using FBDropshipper.Persistence.Context;
using FluentValidation;
using MediatR;

namespace FBDropshipper.Application.Images.Commands.DeleteImages;

public class DeleteImagesRequestModel : IRequest<DeleteImagesResponseModel>
{
    public string[] Urls { get; set; }
}

public class DeleteImagesRequestModelValidator : AbstractValidator<DeleteImagesRequestModel>
{
    public DeleteImagesRequestModelValidator()
    {
    }
}

public class
    DeleteImagesRequestHandler : IRequestHandler<DeleteImagesRequestModel, DeleteImagesResponseModel>
{

    private readonly IImageService _imageService;

    public DeleteImagesRequestHandler(IImageService imageService)
    {
        _imageService = imageService;
    }

    public async Task<DeleteImagesResponseModel> Handle(DeleteImagesRequestModel request,
        CancellationToken cancellationToken)
    {
        foreach (var url in request.Urls)
        {
            await _imageService.DeleteImage(url);
        }

        return new DeleteImagesResponseModel();
    }

}

public class DeleteImagesResponseModel
{

}