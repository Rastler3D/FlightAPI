using Infrastructure.Services;

namespace Flight.Tests.Infrastructure.Services;

public sealed class PasswordServiceTests
{
    private readonly PasswordService _passwordService;

    public PasswordServiceTests()
    {
        _passwordService = new PasswordService();
    }

    [Fact]
    public void HashPassword_ShouldReturnValidBCryptHash()
    {
        // Arrange
        var password = "testPassword123";

        // Act
        var hash = _passwordService.HashPassword(password);

        // Assert
        Assert.NotNull(hash);
        Assert.StartsWith("$2a$", hash); // BCrypt hash format
        Assert.True(hash.Length >= 60); // BCrypt hashes are typically 60 characters
    }

    [Fact]
    public void HashPassword_SamePlaintext_ShouldProduceDifferentHashes()
    {
        // Arrange
        var password = "testPassword123";

        // Act
        var hash1 = _passwordService.HashPassword(password);
        var hash2 = _passwordService.HashPassword(password);

        // Assert
        Assert.NotEqual(hash1, hash2); // BCrypt includes random salt
    }

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "testPassword123";
        var hash = _passwordService.HashPassword(password);

        // Act
        var result = _passwordService.VerifyPassword(password, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "testPassword123";
        var wrongPassword = "wrongPassword";
        var hash = _passwordService.HashPassword(password);

        // Act
        var result = _passwordService.VerifyPassword(wrongPassword, hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyPassword_WithInvalidHash_ShouldReturnFalse()
    {
        // Arrange
        var password = "testPassword123";
        var invalidHash = "invalid_hash";

        // Act
        var result = _passwordService.VerifyPassword(password, invalidHash);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("short")]
    [InlineData("verylongpasswordwithmanycharsandnumbers123456789")]
    public void HashPassword_WithVariousPasswordLengths_ShouldWork(string password)
    {
        // Act
        var hash = _passwordService.HashPassword(password);
        var isValid = _passwordService.VerifyPassword(password, hash);

        // Assert
        Assert.True(isValid);
    }
}
