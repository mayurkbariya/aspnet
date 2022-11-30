using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Common.Constants;
using FBDropshipper.Domain.Entities;
using FBDropshipper.Persistence.Context;
using FBDropshipper.Persistence.Extension;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace FBDropshipper.WebApi.Controllers
{
    public class AuthorizationController : Controller
    {
        private IMediator _mediator;

        protected IMediator Mediator =>
            _mediator ??= (IMediator) HttpContext.RequestServices.GetService(typeof(IMediator));

        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public AuthorizationController(
            IOptions<IdentityOptions> identityOptions,
            SignInManager<User> signInManager,
            UserManager<User> userManager, ApplicationDbContext context)
        {
            _identityOptions = identityOptions;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Authorization
        /// </summary>
        /// <returns>Token</returns>
        /// <exception cref="BadRequestException"></exception>
        [HttpPost("~/connect/token")]
        [Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var requestObject = HttpContext.GetOpenIddictServerRequest() ??
                                throw new InvalidOperationException("The request cannot be retrieved.");
            var role = "";
            if (requestObject.TryGetParameter("role", out var roleParameter))
            {
                if (roleParameter.Value != null)
                {
                    role = roleParameter.Value.ToString();
                }
            }

            if (requestObject.IsPasswordGrantType())
            {
                User user = await _context.Users.GetByAsync(p => p.Email == requestObject.Username &&
                                                                 p.IsEnabled, p => 
                    p.Include(pr => pr.UserClaims)
                        .Include(pr => pr.TeamMemberPermissions)
                        .Include(pr => pr.TeamMember.Team));
                if (user == null)
                {
                    throw new BadRequestException("Please check that your credentials are correct");
                }

                // Validate the username/password parameters and ensure the account is not locked out.
                var result = await _signInManager.CheckPasswordSignInAsync(user, requestObject.Password, true);

                // Ensure the user is enabled.
                if (!user.IsEnabled)
                {
                    throw new BadRequestException("The specified user account has been blocked");
                }
                if (user.TeamMember is { CanLogin: false })
                {
                    throw new BadRequestException("The specified user account has been blocked");
                }
                // Ensure the user is not already locked out.
                if (result.IsLockedOut)
                {
                    throw new BadRequestException("The specified user account has been suspended");
                }

                // Ensure the user is allowed to sign in.
                if (result.IsNotAllowed)
                {
                    throw new BadRequestException("The specified user is not allowed to sign in");
                }

                if (!result.Succeeded)
                {
                    throw new BadRequestException("Please check that your credentials are correct");
                }


                // Create a new authentication ticket.
                var ticket = CreateTicket(requestObject, user);
                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }
            else if (requestObject.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // Retrieve the user profile corresponding to the refresh token.
                // Note: if you want to automatically invalidate the refresh token
                // when the user password/roles change, use the following line instead:
                // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
                User user = await _context.Users.GetByAsync(
                    p => p.Id == info.Principal.FindFirstValue(CustomClaimTypes.UserId),
                    p =>
                        p.Include(pr => pr.UserClaims)
                            .Include(pr => pr.TeamMemberPermissions)
                            .Include(pr => pr.TeamMember.Team)
                );
                if (user == null)
                {
                    throw new BadRequestException("The refresh token is no longer valid");
                }

                // Ensure the user is enabled.
                if (!user.IsEnabled)
                {
                    throw new BadRequestException("The specified user account has been blocked");
                }

                // Ensure the user is still allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    throw new BadRequestException("The user is no longer allowed to sign in");
                }

                // Create a new authentication ticket, but reuse the properties stored
                // in the refresh token, including the scopes originally granted.
                var ticket = CreateTicket(requestObject, user);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            throw new BadRequestException("The specified grant type is not supported");
        }

        private AuthenticationTicket CreateTicket(OpenIddictRequest request, User user)
        {
            // Create a new ClaimsPrincipal containing the claims that
            // will be used to create an id_token, a token or a code.
            //            var principal = _signInManager.CreateUserPrincipalAsync(user).Result;
            //            var claims = _userManager.GetClaimsAsync(user).Result;
            var roles = _userManager.GetRolesAsync(user).Result;
            var claims = new List<Claim>();
            claims.AddRange(roles.Select(p => new Claim(ClaimTypes.Role, p)).ToArray());
            claims.AddRange(user.UserClaims.Select(p => new Claim(p.ClaimType,p.ClaimValue)));
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Basic"));
            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            ticket.Principal.SetAccessTokenLifetime(TimeSpan.FromMinutes(60));
            ticket.Principal.SetIdentityTokenLifetime(TimeSpan.FromMinutes(60));
            ticket.Principal.SetRefreshTokenLifetime(TimeSpan.FromHours(24));
            //if (!request.IsRefreshTokenGrantType())
            //{
            // Set the list of scopes granted to the client application.
            // Note: the offline_access scope must be granted
            // to allow OpenIddict to return a refresh token.
            ticket.Principal.SetScopes(new[]
            {
                OpenIddictConstants.Scopes.OpenId,
                OpenIddictConstants.Scopes.Email,
                OpenIddictConstants.Scopes.Phone,
                OpenIddictConstants.Scopes.Profile,
                OpenIddictConstants.Scopes.OfflineAccess,
                OpenIddictConstants.Scopes.Roles
            }.Intersect(request.GetScopes()));
            //}

            //ticket.SetResources("quickapp-api");

            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.
            foreach (var claim in ticket.Principal.Claims)
            {
                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
                    continue;


                var destinations = new List<string> {OpenIddictConstants.Destinations.AccessToken};

                // Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
                // The other claims will only be added to the access_token, which is encrypted when using the default format.
                if ((claim.Type == OpenIddictConstants.Claims.Subject &&
                     ticket.Principal.HasScope(OpenIddictConstants.Scopes.OpenId)) ||
                    (claim.Type == OpenIddictConstants.Claims.Name &&
                     ticket.Principal.HasScope(OpenIddictConstants.Scopes.Profile)) ||
                    (claim.Type == OpenIddictConstants.Claims.Role &&
                     ticket.Principal.HasScope(OpenIddictConstants.Claims.Role)))
                {
                    destinations.Add(OpenIddictConstants.Destinations.IdentityToken);
                }

                claim.SetDestinations(destinations);
            }

           

            if (!(principal.Identity is ClaimsIdentity identity))
            {
                throw new Exception("Error: Principle is Null");
            }
            if (user.TeamMemberPermissions.Any())
            {
                foreach (var permission in user.TeamMemberPermissions)
                {
                    identity.AddClaim(CustomClaimTypes.MarketPlace, 
                        permission.MarketPlaceId.ToString(),
                        OpenIddictConstants.Destinations.IdentityToken,
                        OpenIddictConstants.Destinations.AccessToken);
                }
            }
            identity.AddClaim(OpenIddictConstants.Claims.Subject, user.Email,
                OpenIddictConstants.Destinations.IdentityToken);
            identity.AddClaim(CustomClaimTypes.UserId, user.Id, OpenIddictConstants.Destinations.IdentityToken,
                OpenIddictConstants.Destinations.AccessToken);
            if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
                identity.AddClaim(CustomClaimTypes.Phone, user.PhoneNumber,
                    OpenIddictConstants.Destinations.IdentityToken);


            if (user.UserName != null)
            {
                identity.AddClaim(CustomClaimTypes.UserName, user.UserName,
                    OpenIddictConstants.Destinations.IdentityToken);
            }

            if (user.TeamMember is { Team: { } })
            {
                identity.AddClaim(CustomClaimTypes.TeamLeaderId, user.TeamMember.Team.UserId,
                    OpenIddictConstants.Destinations.IdentityToken, OpenIddictConstants.Destinations.AccessToken);
                identity.AddClaim(CustomClaimTypes.TeamId, user.TeamMember.Team.Id.ToString(),
                    OpenIddictConstants.Destinations.IdentityToken, OpenIddictConstants.Destinations.AccessToken);

            }


            foreach (var role in roles)
            {
                if (user.IsEnabled)
                {
                    identity.AddClaim(OpenIddictConstants.Claims.Role, role,
                        OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken);
                }
                else
                {
                    identity.AddClaim(OpenIddictConstants.Claims.Role, role,
                        OpenIddictConstants.Destinations.IdentityToken);
                }
            }

            if (ticket.Principal.HasScope(OpenIddictConstants.Scopes.Profile))
            {
                //                if (!string.IsNullOrWhiteSpace(user.JobTitle))
                //                    identity.AddClaim(CustomClaimTypes.JobTitle, user.JobTitle, OpenIddictConstants.Destinations.IdentityToken);
                //
                if (user.FullName != null)
                {
                    identity.AddClaim(CustomClaimTypes.FullName, user.FullName,
                        OpenIddictConstants.Destinations.IdentityToken);
                }
                //
                //                if (!string.IsNullOrWhiteSpace(user.Configuration))
                //                    identity.AddClaim(CustomClaimTypes.Configuration, user.Configuration, OpenIddictConstants.Destinations.IdentityToken);
            }

            if (ticket.Principal.HasScope(OpenIddictConstants.Scopes.Email))
            {
                if (!string.IsNullOrWhiteSpace(user.Email))
                    identity.AddClaim(CustomClaimTypes.Email, user.Email,
                        OpenIddictConstants.Destinations.IdentityToken);
            }

            return ticket;
        }
    }
}