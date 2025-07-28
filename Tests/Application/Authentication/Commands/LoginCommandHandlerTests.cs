using Application.Authentication.Commands.Login;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Constants;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Flight.Tests.Application.Authentication.Commands;

public sealed class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<ILogger<LoginCommandHandler>> _loggerMock;
    private readonly AuthenticationService _authenticationService;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _authenticationService = new AuthenticationService(_userRepositoryMock.Object, _passwordServiceMock.Object,
            _tokenServiceMock.Object, new Mock<ILogger<AuthenticationService>>().Object);
        _loggerMock = new Mock<ILogger<LoginCommandHandler>>();
        _handler = new LoginCommandHandler(
            _authenticationService,
            _loggerMock.Object);
    }
    

    [Fact]
    public async Task Handle_WithInvalidPassword_ShouldReturnFailure()
    {
        
        var command = new LoginCommand("testuser", "wrongpassword");
        var user = new User("testuser", "hashedpassword", 1);

        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(command.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<User?>(user));

        _passwordServiceMock
            .Setup(x => x.VerifyPassword(command.Password, user.PasswordHash))
            .Returns(false);

        
        var result = await _handler.Handle(command, CancellationToken.None);

        
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_ShouldReturnFailure()
    {
        
        var command = new LoginCommand("nonexistent", "password123");

        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync(command.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<User?>(null));

        
        var result = await _handler.Handle(command, CancellationToken.None);

        
        Assert.True(result.IsFailure);
    }
}