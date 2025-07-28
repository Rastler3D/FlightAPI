using Domain.Common;

namespace Domain.Entities;

public sealed class Role : Entity
{
    public string Code { get; private set; } = string.Empty;
    public ICollection<User> Users { get; private set; } = new List<User>();

    private Role() { } // EF Core

    public Role(string code)
    {
        Code = code;
    }

    public static class Codes
    {
        public const string User = "User";
        public const string Moderator = "Moderator";
    }
}
