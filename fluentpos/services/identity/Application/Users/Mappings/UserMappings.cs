using FluentPos.Identity.Application.Users.Dtos;
using FluentPos.Identity.Domain.Users;
using Mapster;

namespace FluentPos.Identity.Application.Users.Mappings;
public sealed class UserMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AppUser, UserDto>();
    }
}
