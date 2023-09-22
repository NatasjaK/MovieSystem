using Microsoft.EntityFrameworkCore;
using MovieSystem.RepositoryPattern;
using Microsoft.AspNetCore;
using MovieSystem.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection.PortableExecutable;
using System;

var builder = WebApplication.CreateBuilder(args);

// https://api.themoviedb.org/3/movie/550?api_key=4aeb86e0014b8416de3595b985066874
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure CORS (Cross-Origin Resource Sharing) for the application services.
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    // Allow requests from any origin (all domains and origins).
    builder.WithOrigins("*")
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

// Configure the application to use Entity Framework Core to connect to a SQL Server database.
builder.Services.AddDbContext<MovieDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("corsapp");
app.UseAuthorization();

// Define routes and handlers
app.MapGet("/myroute", async (HttpContext httpContext) =>
{
    await httpContext.Response.SendFileAsync("TestPostRequest.html");
});

// Get all people
app.MapGet("/Get/People", async (MovieDbContext movieDbContext) =>
{
    var people = await movieDbContext.Persons.ToListAsync();
    return Results.Ok(people);  
})
.WithName("GetAllPeople");

// Get all genres
app.MapGet("/Get/Genres", async (MovieDbContext movieDbContext) =>
{
    var genres = await movieDbContext.Genres.ToListAsync();
    return Results.Ok(genres);
})
.WithName("GetAllGenres");

// Get genre for specific person
app.MapGet("/Get/PersonGenre/{Id}", async (int Id, MovieDbContext movieDbContext) =>
{
    // Query the database to retrieve liked genres for the person with the specified Id
    var personGenres = movieDbContext.LikedGenres
        .Where(pg => pg.Person.Id == Id)
        .Select(pg => new { pg.Person.Id, pg.Genre.Title })
        .ToListAsync();
    return Results.Ok(await personGenres);
})
 // Set a custom route name for this endpoint
 .WithName("GetGenrebyPersonId");

// Get movies for specific person
app.MapGet("/Get/PersonMovie/{Id}", async (int Id, MovieDbContext context) =>
{
    var personMovies = context.LikedGenres
        .Where(pg => pg.PersonId == Id)
        .Select(pg => new { pg.Person.Id, pg.Movie })
        .ToListAsync();
    return Results.Ok(await personMovies);
})
.WithName("GetMoviebyPersonId");


// Get rating for specific person and movie
app.MapGet("/Get/MovieRating/{Id}", async (int Id, MovieDbContext context) =>
{
    var movieRatings = context.LikedGenres
        .Where(pg => pg.PersonId == Id)
        .Select(pg => new { pg.Person.Id, pg.Movie, pg.Rating })
        .ToListAsync();
    return Results.Ok(await movieRatings);
})
.WithName("GetMovieRatingbyPersonId");

// Add/Update rating with personId and movie
app.MapPost("/Post/AddRating", async (HttpContext httpContext, int personId, int rating, string movie, MovieDbContext context) =>
{
    var update = context.LikedGenres
    .FirstOrDefault(pg => pg.PersonId == personId && pg.Movie == movie);

    // If a matching record is found, update the rating and save changes to the database
    if (update != null)
    {
        update.Rating = rating;
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
})
.WithName("PostRatingByPersonIdAndMovie");

// Add genre to person
app.MapPost("/Post/AddGenre", async (int personId, int genreId, MovieDbContext context) =>
{
    var response = new LikedGenre
    {
        PersonId = personId,
        GenreId = genreId
    };
    context.LikedGenres.Add(response);
    await context.SaveChangesAsync();
    return Results.Created($"/Get/PersonGenre/{personId}", response);
})
.WithName("PostGenreByPersonIdAndGenreId");

// Get Recommendations based on genre
app.MapGet("/Get/Recommendations", async (string genreTitle, MovieDbContext context) =>
{
    var genre = await context.Genres.FirstOrDefaultAsync(g => g.Title == genreTitle);

    if (genre != null)
    {
        var apiKey = "4aeb86e0014b8416de3595b985066874";
        var url = $"https://api.themoviedb.org/3/discover/movie?api_key={apiKey}&sort_by=popularity.desc&include_adult=true&include_video=false&with_genres={genre.Id}&with_watch_monetization_types=free";

        var client = new HttpClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        return Results.Content(content, contentType: "application/json");
    }

    return Results.NotFound();
});
/*// Get all links for a specific user and genre
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
app.MapPost("/Add/UserGenreLink", async (LikedGenre link, MovieDbContext context) =>
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

// Configure HttpClient with the base address of the API
var apiKey = "4aeb86e0014b8416de3595b9850668744aeb86e0014b8416de3595b985066874"; // Your TMDb API key
var baseUrl = "https://api.themoviedb.org/3/";
var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri(baseUrl);

// GET movie suggestions in a specific genre from TMDb API
app.MapGet("/Get/MovieSuggestions/{GenreId}", async (string GenreId, HttpContext httpContext) =>
{
    try
    {
        // Construct the API URL with the provided GenreId and API key
        string apiUrl = $"discover/movie?api_key={apiKey}&with_genres={GenreId}";

        // Send a GET request to the TMDb API
        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

        // Check if the request was successful
        if (response.IsSuccessStatusCode)
        {
            // Deserialize the response content to a string (you can use JSON deserialization if needed)
            string responseContent = await response.Content.ReadAsStringAsync();

            // Return the response content as a JSON string
            return Results.Json(responseContent);
        }
        else
        {
            // Handle the API error if needed
            // You can log the error or return a custom error message
            return Results.BadRequest("Failed to fetch movie suggestions.");
        }
    }
    catch (HttpRequestException)
    {
        // Handle exceptions if the request fails
        return Results.BadRequest("Failed to connect to the external API.");
    }
})
.WithName("GetMovieSuggestions");

*/



app.Run();
