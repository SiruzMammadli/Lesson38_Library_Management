namespace Lesson38.Application.Services;

public sealed class BookService(AppDbContext db)
{
    public List<Book> GetBooks()
    {
        var books = db.Books.ToList();
        return books;
    }

    public Book? GetBookById(int id)
    {
        var book = db.Books.FirstOrDefault(b => b.Id == id);
        return book;
    }

    public void AddBook(Book book)
    {
        db.Books.Add(book);
        db.SaveChanges();
    }

    public void UpdateBook(Book book)
    {
        db.Books.Update(book);
        db.SaveChanges();
    }

    public void DeleteBook(Book book)
    {
        db.Books.Remove(book);
        db.SaveChanges();
    }
}
