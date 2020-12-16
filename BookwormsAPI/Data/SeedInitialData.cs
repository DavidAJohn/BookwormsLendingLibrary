using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BookwormsAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookwormsAPI.Data
{
    public class SeedInitialData
    {
        public static async Task SeedAuthorsDataAsync(ApplicationDbContext context)
        {
            if (await context.Authors.AnyAsync()) return;

            var authorsData = await File.ReadAllTextAsync("./Data/SeedData/authors.json");
            var authors = JsonSerializer.Deserialize<List<Author>>(authorsData);

            foreach (var author in authors)
            {
                context.Authors.Add(author);
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedCategoriesDataAsync(ApplicationDbContext context)
        {
            if (await context.Categories.AnyAsync()) return;
            
            var categoriesData = await File.ReadAllTextAsync("./Data/SeedData/categories.json");
            var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

            foreach (var category in categories)
            {
                context.Categories.Add(category);
            }

            await context.SaveChangesAsync();
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