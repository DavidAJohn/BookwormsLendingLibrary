using System;
using BookwormsAPI.Entities;
using BookwormsAPI.Entities.Borrowing;
using Microsoft.EntityFrameworkCore;

namespace BookwormsAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Request> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId);

            // creates a relationship between Request and Address without needing a navigation property
            modelBuilder.Entity<Request>()
                .OwnsOne(r => r.SendToAddress, a => {
                    a.WithOwner();
                });

            // converts the RequestStatus enum into a string
            modelBuilder.Entity<Request>()
                .Property(s => s.Status)
                .HasConversion(
                    rs => rs.ToString(),
                    rs => (RequestStatus) Enum.Parse(typeof(RequestStatus), rs)
                );
        }
    }
}