using Microsoft.EntityFrameworkCore;
using MovieSystem.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

public class MovieDbContext : DbContext
{
    public DbSet<Person> People { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Link> Links { get; set; }

    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Title = "Science Fiction", Description = "Explore futuristic worlds, advanced technology, and the impact of scientific advancements on society and humanity." },
            new Genre { Id = 2, Title = "Fantasy", Description = "Immerse yourself in magical realms, mythical creatures, and epic adventures beyond the boundaries of reality." },
            new Genre { Id = 3, Title = "Mystery", Description = "Solve puzzles, unravel enigmatic plots, and uncover hidden secrets in thrilling and suspenseful narratives." },
            new Genre { Id = 4, Title = "Romance", Description = "Experience the passion, love, and emotional rollercoaster of relationships and romantic encounters." },
            new Genre { Id = 5, Title = "Horror", Description = "Face your deepest fears as you confront supernatural entities, psychological terrors, and the darkest aspects of human nature." },
            new Genre { Id = 6, Title = "Adventure", Description = "Embark on daring quests, explore uncharted territories, and engage in thrilling escapades across various landscapes." },
            new Genre { Id = 7, Title = "Historical Fiction", Description = "Travel through time and delve into the past, experiencing historical events and periods through the eyes of fictional characters." },
            new Genre { Id = 8, Title = "Thriller", Description = "Experience suspense, tension, and high-stakes situations in fast-paced narratives that keep you on the edge of your seat." },
            new Genre { Id = 9, Title = "Comedy", Description = "Find humor in everyday situations, witty dialogues, and humorous characters that make you laugh out loud." },
            new Genre { Id = 10, Title = "Drama", Description = "Explore complex emotions, interpersonal relationships, and the human condition in emotionally charged and thought-provoking stories." }
        );
    }

}
