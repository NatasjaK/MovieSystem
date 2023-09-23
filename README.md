# MovieSystem 
This is a simple web API project developed using C# and .NET Core 6, specifically Minimal API. It uses both a local database for storing user and genre information and an external API (https://www.themoviedb.org/) to provide movie recommendations based on genres.

## 📞 API Endpoints

> `​/Get/People` - Get all people in the system.
>
> `​/Get/Genres` - Get all genres in the system.
>
> `​/Get/PersonGenre/{Id}` - Get genre for specific person.
>
> `​/Get/PersonMovie/{Id}` - Get movies for specific person.
>
> `/​Get/MovieRating/{Id}` - Get rating for specific person and movie.
>
> `​/Post/AddRating` - Add/Update rating with personId and movie.
>
> `​/Post/AddGenre` - Add genre to person.
>
> `/Get/Recommendations` - Get Recommendations based on genre using TMDB.

## Installation
Clone the repository from GitHub:

Clone this repository: git clone https://github.com/your-username/MovieSystem.git 

Open the project in Visual Studio.

Ensure that you have .NET Core 6 installed.

Build and run the project.





Feel free to contribute, report issues, or provide feedback. Thank you for using the MovieSystem API!
