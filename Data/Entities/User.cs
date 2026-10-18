namespace Lesson38.Data.Entities;

public sealed class User : BaseEntity
{
    public string? Fullname { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public required string Password { get; set; }
}