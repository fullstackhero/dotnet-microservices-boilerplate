using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using IDS.Host.Models;
using Microsoft.AspNetCore.Identity;

namespace IDS.Host.Validators;


public class UserValidator : IResourceOwnerPasswordValidator
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserValidator(SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var user = await _userManager.FindByNameAsync(context.UserName);
        if (user is not null)
        {
            var signIn = await _signInManager.PasswordSignInAsync(
            user!,
            context.Password,
            isPersistent: true,
            lockoutOnFailure: true);

            if (signIn.Succeeded)
            {
                var userId = user!.Id.ToString();
                context.Result = new GrantValidationResult(
                    subject: userId,
                    authenticationMethod: "custom",
                    claims: new Claim[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, user.UserName!)
                    }
                );

                return;
            }
        }
        context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, "Invalid Credentials");
    }
}
