using FBDropshipper.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BaseController : Controller
    {
        private IMediator _mediator;
        private UserManager<User> _userManager;
        protected UserManager<User> UserManager => _userManager ??=
            (UserManager<User>) HttpContext.RequestServices.GetService(
                typeof(UserManager<User>));
        protected IMediator Mediator =>
            _mediator ??= (IMediator) HttpContext.RequestServices.GetService(typeof(IMediator));
    }
}