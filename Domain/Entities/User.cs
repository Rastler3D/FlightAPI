using Domain.Common;

namespace Domain.Entities;

public sealed class User : BaseEntity
{
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public int RoleId { get; private set; }
    public Role Role { get; private set; } = null!;

    private User() { } // EF Core

    public User(string username, string passwordHash, int roleId)
    {
        Username = username;
        PasswordHash = passwordHash;
        RoleId = roleId;
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
}
