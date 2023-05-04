using FluentPOS.Identity.Core.Users.Dtos;
using Mapster;

namespace FluentPOS.Identity.Core.Users.Mappings;
public sealed class UserMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AppUser, UserDto>();
    }
}
