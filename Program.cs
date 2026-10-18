//dotnet tool install--global dotnet-ef     -- mutleq terminalda isledilib sadece bir defe yuklenmeli

using Lesson38.Application.Services;
using Microsoft.Extensions.Logging;
using System.Text;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var config = new ConfigurationBuilder()
    .SetBasePath("C:\\Users\\USER\\Desktop\\CODEACADEMY\\PA403\\Lesson38")
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var builder = new ServiceCollection();

builder.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("Default"))
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging(); ;
});
builder.AddScoped<CategoryService>();
builder.AddScoped<BookService>();

var provider = builder.BuildServiceProvider();
var categoryService = provider.GetRequiredService<CategoryService>();
var bookService = provider.GetRequiredService<BookService>();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. Kateqoriyalara bax");
    Console.WriteLine("2. Yeni kateqoriya əlavə edin");
    Console.WriteLine("3. Kateqoriyanı yenilə");
    Console.WriteLine("4. Kateqoriyanı sil");
    Console.WriteLine("5. Kitablara bax");
    Console.WriteLine("6. Yeni kitab əlavə edin");
    Console.WriteLine("7. Kitabı yenilə");
    Console.WriteLine("8. Kitabı sil");
    Console.WriteLine("0. Çıxış");
    Console.Write("Seçim: ");

    switch (Console.ReadLine())
    {
        case "1":
            foreach (var c in categoryService.GetCategories())
                Console.WriteLine($"{c.Id}. {c.Name}");
            break;

        case "2":
            Console.Write("Kateqoriya adı: ");
            var name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Ad boş ola bilməz.");
                break;
            }
            categoryService.AddCategory(new Category { Name = name.Trim() });
            Console.WriteLine("Kateqoriya əlavə edildi.");
            break;

        case "3":
            var toUpdate = FindCategory();
            if (toUpdate is null) break;
            Console.Write("Yeni ad: ");
            var newName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newName))
            {
                Console.WriteLine("Ad boş ola bilməz.");
                break;
            }
            toUpdate.Name = newName.Trim();
            categoryService.UpdateCategory(toUpdate);
            Console.WriteLine("Kateqoriya yeniləndi.");
            break;

        case "4":
            var toDelete = FindCategory();
            if (toDelete is null) break;
            categoryService.DeleteCategory(toDelete);
            Console.WriteLine("Kateqoriya silindi.");
            break;

        case "5":
            foreach (var b in bookService.GetBooks())
                Console.WriteLine($"{b.Id}. {b.Title} - {b.Price} AZN (Kateqoriya Id: {b.CategoryId})");
            break;

        case "6":
            var newBook = new Book();
            if (!ReadBookFields(newBook)) break;
            bookService.AddBook(newBook);
            Console.WriteLine("Kitab əlavə edildi.");
            break;

        case "7":
            var bookToUpdate = FindBook();
            if (bookToUpdate is null || !ReadBookFields(bookToUpdate)) break;
            bookService.UpdateBook(bookToUpdate);
            Console.WriteLine("Kitab yeniləndi.");
            break;

        case "8":
            var bookToDelete = FindBook();
            if (bookToDelete is null) break;
            bookService.DeleteBook(bookToDelete);
            Console.WriteLine("Kitab silindi.");
            break;

        case "0":
            return;

        default:
            Console.WriteLine("Yanlış seçim.");
            break;
    }
}

Category? FindCategory()
{
    Console.Write("Kateqoriya Id: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Id rəqəm olmalıdır.");
        return null;
    }
    var category = categoryService.GetCategoryById(id);
    if (category is null) Console.WriteLine("Kateqoriya tapılmadı.");
    return category;
}

Book? FindBook()
{
    Console.Write("Kitab Id: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Id rəqəm olmalıdır.");
        return null;
    }
    var book = bookService.GetBookById(id);
    if (book is null) Console.WriteLine("Kitab tapılmadı.");
    return book;
}

// Fills book only if every field is valid, so a failed update leaves it untouched.
bool ReadBookFields(Book book)
{
    Console.Write("Kitab adı: ");
    var title = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(title))
    {
        Console.WriteLine("Ad boş ola bilməz.");
        return false;
    }

    Console.Write("Qiymət: ");
    if (!decimal.TryParse(Console.ReadLine(), out var price) || price < 0)
    {
        Console.WriteLine("Qiymət mənfi olmayan rəqəm olmalıdır.");
        return false;
    }

    var category = FindCategory();
    if (category is null) return false;

    book.Title = title.Trim();
    book.Price = price;
    book.CategoryId = category.Id;
    return true;
}