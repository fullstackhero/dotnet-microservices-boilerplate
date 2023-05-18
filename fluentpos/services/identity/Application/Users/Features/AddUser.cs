using FluentPos.Identity.Application.Users.Dtos;
using FluentPos.Identity.Application.Users.Exceptions;
using FluentPos.Identity.Domain.Users;
using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FluentPos.Identity.Application.Users.Features;

public static class AddUser
{
    public sealed record Command : IRequest<UserDto>
    {
        public readonly AddUserDto AddUserDto;
        public Command(AddUserDto addUserDto)
        {
            AddUserDto = addUserDto;
        }
    }
    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.AddUserDto.UserName)
                .NotEmpty()
                .Matches("\\w+").WithMessage("The {0} must only contain letters and numbers")
                .MaximumLength(75)
                .WithName("UserName");

            RuleFor(p => p.AddUserDto.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(p => p.AddUserDto.Password)
                .NotEmpty()
                .MinimumLength(5);
        }
    }
    public sealed class Handler : IRequestHandler<Command, UserDto>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<Command> _logger;

        public Handler(UserManager<AppUser> userManager, IMapper mapper, ILogger<Command> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var userWithSameName = await _userManager.FindByNameAsync(request.AddUserDto.UserName!);
            if (userWithSameName != null) throw new UserRegistrationException(string.Format("Username {0} is already taken.", request.AddUserDto.UserName));
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.AddUserDto.Email!);
            if (userWithSameEmail != null) throw new UserRegistrationException(string.Format("Email {0} is already registered.", request.AddUserDto.Email));

            AppUser user = new() { UserName = request.AddUserDto.UserName, Email = request.AddUserDto.Email };
            var result = await _userManager.CreateAsync(user, request.AddUserDto.Password!);
            if (result.Succeeded)
            {
                return _mapper.Map<UserDto>(user);
            }

            foreach (var error in result.Errors)
            {
                _logger.LogError("{error}", error.Description);
            }

            throw new UserRegistrationException("Identity Exception");
        }
    }
}
