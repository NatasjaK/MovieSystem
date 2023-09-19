using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieSystem.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MovieDbContext>(options =>
{
    options.UseSqlServer("DefaultConnection"); 
});
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Get all people
app.MapGet("/Get/People", async (MovieDbContext context) =>
{
    // Retrieve all people from the database
    var people = context.People;
    return await people.ToListAsync();
})
.WithName("GetAllPeople");

// Get all genres
app.MapGet("/Get/Genres", async (MovieDbContext context) =>
{
    // Retrieve all genres from the database
    var genres = context.Genres;
    return await genres.ToListAsync();
})
.WithName("GetAllGenres");

// Get all genres associated with a specific person
app.MapGet("/Get/PersonGenres/{PersonId}", async (int PersonId, MovieDbContext context) =>
{
    // Query the database to retrieve genres associated with a specific person
    var genres = from link in context.Links
                 join genre in context.Genres on link.GenreId equals genre.Id
                 where link.PersonId == PersonId
                 select genre;

    return await genres.Distinct().ToListAsync();
})
.WithName("GetPersonGenres");

// Get all links for a specific user and genre
app.MapGet("/Get/UserGenreLinks/{UserId}/{GenreId}", async (int UserId, int GenreId, MovieDbContext context) =>
{
    // Query the database to retrieve links for a specific user and genre
    var links = from link in context.Links
                where link.PersonId == UserId && link.GenreId == GenreId
                select link;

    return await links.ToListAsync();
})
.WithName("GetLinksForUserAndGenre");

// Add a new user
app.MapPost("/Add/User", async (Person person, MovieDbContext context) =>
{
    // Add a new user to the database
    context.People.Add(person);
    await context.SaveChangesAsync();

    // Return a response indicating the user has been created
    return Results.Created($"/Get/User/{person.Id}", person);
})
.WithName("AddUser");

// Add a new genre
app.MapPost("/Add/Genre", async (Genre genre, MovieDbContext context) =>
{
    // Add a new genre to the database
    context.Genres.Add(genre);
    await context.SaveChangesAsync();

    // Return a response indicating the genre has been created
    return Results.Created($"/Get/Genres/{genre.Id}", genre);
})
.WithName("AddGenre");

// Add a new link for a specific user and genre
app.MapPost("/Add/UserGenreLink", async (Link link, MovieDbContext context) =>
{
    // Add a new link to the database for a specific user and genre
    context.Links.Add(link);
    await context.SaveChangesAsync();

    // Return a response indicating the link has been created
    return Results.Created($"/Get/UserGenreLinks/{link.PersonId}/{link.GenreId}/{link.Id}", link);
})
.WithName("AddLink");

// Get rating for a specific user and movie
app.MapGet("/Get/MovieRating/{UserId}/{MovieId}", async (int UserId, int MovieId, MovieDbContext context) =>
{
    // Query the database to retrieve the movie rating for a specific user and movie
    var movieRating = from link in context.Links
                      where link.PersonId == UserId && link.Movie != null && link.Id == MovieId
                      select new
                      {
                          UserId = link.PersonId,
                          Movie = link.Movie,
                          Rating = link.Rating
                      };

    // Return the movie rating as a list
    return await movieRating.ToListAsync();
})
.WithName("GetMovieRating");

// GET movie ratings for a specific person
app.MapGet("/Get/MovieRatings/{UserId}", async (int UserId, MovieDbContext context) =>
{
    // Query the database to retrieve movie ratings for a specific user
    var movieRatings = await context.Links
        .Where(link => link.PersonId == UserId && link.Movie != null && link.Rating != null)
        .Select(link => new
        {
            Movie = link.Movie,
            Rating = link.Rating.Value // Assuming Rating is nullable int
        })
        .ToListAsync();

    return movieRatings;
})
.WithName("GetMovieRatings");





app.UseHttpsRedirection();


app.Run();
