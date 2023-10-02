using BookwormsAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BookwormsAPI.Data
{
    public class SeedInitialData
    {
        public static async Task SeedAuthorsDataAsync(ApplicationDbContext context)
        {
            if (await context.Authors.AnyAsync()) return;

            var authorsData = await File.ReadAllTextAsync("./Data/SeedData/authors.json");
            var authors = JsonSerializer.Deserialize<List<Author>>(authorsData);

            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = context.Database.BeginTransaction();

                foreach (var author in authors)
                {
                    context.Authors.Add(author);
                }

                context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT Authors ON;");
                await context.SaveChangesAsync();
                context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT Authors OFF");
                transaction.Commit();
            });
        }

        public static async Task SeedCategoriesDataAsync(ApplicationDbContext context)
        {
            if (await context.Categories.AnyAsync()) return;
            
            var categoriesData = await File.ReadAllTextAsync("./Data/SeedData/categories.json");
            var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = context.Database.BeginTransaction();

                foreach (var category in categories)
                {
                    context.Categories.Add(category);
                }

                context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT Categories ON;");
                await context.SaveChangesAsync();
                context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT Categories OFF");
                transaction.Commit();
            });
        }

        public static async Task SeedBooksDataAsync(ApplicationDbContext context)
        {
            if (await context.Books.AnyAsync()) return;

            var booksData = await File.ReadAllTextAsync("./Data/SeedData/books.json");
            var books = JsonSerializer.Deserialize<List<Book>>(booksData);

            foreach (var book in books)
            {
                context.Books.Add(book);
            }

            await context.SaveChangesAsync();
        }

    }
}