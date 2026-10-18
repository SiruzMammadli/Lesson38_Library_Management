namespace Lesson38.Data.Entities;

public sealed class Book : BaseEntity
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
}