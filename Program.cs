using Microsoft.EntityFrameworkCore;
using MovieSystem.RepositoryPattern;
using Microsoft.AspNetCore;
using MovieSystem.Models;

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

/*// Get all genres
app.MapGet("/Get/Genres", (HttpContext httpContext) =>
{
    var genreRepo = new GenreRepository(httpContext.RequestServices.GetRequiredService<MovieDbContext>());
    var genres = genreRepo.GetAll();
    return genres;
})
.WithName("GetAllGenres");

// Add a person

app.MapPost("/Add/Person", (Person person) =>
{
    MovieDbContext movieContext = new MovieDbContext();
    PersonRepository personRepo = new PersonRepository(movieContext);
    personRepo.Create(person);
    movieContext.SaveChanges();
    return person;

}).WithName("AddPerson");
*/


/*
//Get genre for specific user
app.MapGet("/Get/UserGenre/", async (int Id, MovieDbContext context) =>
{
    var userGenre = from x in context.UserGenre
                    select new
                    {
                        x.User.Id,
                        x.Genre.Title
                    };

    return await userGenre.Where(x => x.Id == Id).ToListAsync();
})
.WithName("/Get/GenrebyUserId");

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
