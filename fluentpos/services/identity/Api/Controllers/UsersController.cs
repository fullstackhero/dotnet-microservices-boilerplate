using FluentPos.Identity.Application.Users.Dtos;
using FluentPos.Identity.Application.Users.Features;
using FSH.Framework.Infrastructure.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluentPos.Identity.Api.Controllers;

public class UsersController : BaseApiController
{
    [HttpPost(Name = nameof(AddUserAsync))]
    [AllowAnonymous]
    public async Task<IActionResult> AddUserAsync(AddUserDto request)
    {
        var command = new AddUser.Command(request);
        var userDto = await Mediator.Send(command);

        return CreatedAtRoute(nameof(AddUserAsync), userDto);
    }

    //[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    //[HttpGet("~/connect/userinfo"), HttpPost("~/connect/userinfo"), Produces("application/json")]
    //public async Task<IActionResult> GetCurrentUserInfoAsync()
    //{
    //    var command = new GetCurrentUserInfo.Query(User);
    //    var userDto = await Mediator.Send(command);

    //    return Ok(userDto);
    //}
}
