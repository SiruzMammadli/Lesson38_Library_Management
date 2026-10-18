namespace Lesson38.Application.Services;

public sealed class CategoryService(AppDbContext db)
{
    public List<Category> GetCategories()
    {
        var categories = db.Categories.ToList();
        return categories;
    }

    public Category? GetCategoryById(int id)
    {
        var category = db.Categories.FirstOrDefault(c => c.Id == id);
        return category;
    }

    public void AddCategory(Category category)
    {
        db.Categories.Add(category);
        db.SaveChanges();
    }

    public void UpdateCategory(Category category)
    {
        db.Categories.Update(category);
        db.SaveChanges();
    }

    public void DeleteCategory(Category category)
    {
        db.Categories.Remove(category);
        db.SaveChanges();
    }
}
