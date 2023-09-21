using MovieSystem.Models;

namespace MovieSystem.RepositoryPattern
{
    public class GenreRepository : RepositoryBase<Genre>, IGenreRepository
    {
        public GenreRepository(MovieDbContext movieDbContext)
            : base(movieDbContext)
        {
        }
       
    }
}
