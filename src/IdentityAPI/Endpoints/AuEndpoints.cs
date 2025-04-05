using System.Security.Claims;
using IdentityAPI.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Shared;

namespace IdentityAPI.Endpoints;

public class AuEndpoints : IEndpoints
{
    public void Map(IEndpointRouteBuilder endpoint)
    {
        var apis = endpoint.MapGroup("connect");
        apis.MapPost("/token", AuEndpointServices.Exchange);
        apis.MapPost("/register", AuEndpointServices.Register);
    }
}

public static class AuEndpointServices
{
    public static async Task<IResult> Exchange(HttpContext context,
        [FromServices] UserManager<ApplicationIdentityUser> userManager)
    {
        var request = context.GetOpenIddictServerRequest();
        if (request is null)
        {
            return Results.BadRequest();
        }

        if (!request.IsPasswordGrantType())
        {
            return Results.InternalServerError("Not supported");
        }

        if (request.Username is null)
        {
            return Results.BadRequest();
        }

        if (request.Password is null)
        {
            return Results.BadRequest();
        }
        var user = await userManager.FindByNameAsync(request.Username);
        if (user is null)
        {
            return Results.BadRequest("Cannot find user");
        }
        var isVerifyPassword = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isVerifyPassword)
        {
            return Results.BadRequest("Invalid password");
        }

        var identity = new ClaimsIdentity(
            authenticationType:TokenValidationParameters.DefaultAuthenticationType,
            nameType: OpenIddictConstants.Claims.Name,
            roleType: OpenIddictConstants.Claims.Role
            );
        identity.SetClaim(OpenIddictConstants.Claims.Subject, user.Id.ToString())
            .SetClaim(OpenIddictConstants.Claims.Name, user.UserName)
            .SetClaim(OpenIddictConstants.Claims.Email, user.Email)
            .SetClaims(OpenIddictConstants.Claims.Role, [.. (await userManager.GetRolesAsync(user))])
            .SetAudiences("https://localhost:7098");
        identity.SetScopes(request.GetScopes());
        return Results.SignIn(new ClaimsPrincipal(identity), null, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    public static async Task<IResult> Register(
        [FromBody] CreateNewUserRequest request,
        [FromServices] UserManager<ApplicationIdentityUser> userManager)
    {
        var createUserResult = await userManager.CreateAsync(new ApplicationIdentityUser()
        {
            UserName = request.Username,
            Email = request.Email,
        }, request.Password);
        if (!createUserResult.Succeeded)
        {
            return Results.BadRequest();
        }
        return Results.Ok();
    }
//     private static IEnumerable<string> GetDestinations(Claim claim)
//     {
//         // Note: by default, claims are NOT automatically included in the access and identity tokens.
//         // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
//         // whether they should be included in access tokens, in identity tokens or in both.
//
//         switch (claim.Type)
//         {
//             case OpenIddictConstants.Claims.Name or OpenIddictConstants.Claims.PreferredUsername:
//                 yield return OpenIddictConstants.Destinations.AccessToken;
//
//                 if (claim.Subject.HasScope(OpenIddictConstants.Permissions.Scopes.Profile))
//                     yield return OpenIddictConstants.Destinations.IdentityToken;
//
//                 yield break;
//
//             case OpenIddictConstants.Claims.Email:
//                 yield return OpenIddictConstants.Destinations.AccessToken;
//
//                 if (claim.Subject.HasScope(OpenIddictConstants.Permissions.Scopes.Email))
//                     yield return OpenIddictConstants.Destinations.IdentityToken;
//
//                 yield break;
//
//             case OpenIddictConstants.Claims.Role:
//                 yield return OpenIddictConstants.Destinations.AccessToken;
//
//                 if (claim.Subject.HasScope(OpenIddictConstants.Permissions.Scopes.Roles))
//                     yield return OpenIddictConstants.Destinations.IdentityToken;
//
//                 yield break;
//
//             // Never include the security stamp in the access and identity tokens, as it's a secret value.
//             case "AspNet.Identity.SecurityStamp": yield break;
//
//             default:
//                 yield return OpenIddictConstants.Destinations.AccessToken;
//                 yield break;
//         }
//     }
}

public record CreateNewUserRequest(string Username, string Password, string Email);
