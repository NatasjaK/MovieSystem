using MovieSystem.Models;

namespace MovieSystem.RepositoryPattern
{
    public class LikedGenreRepository : RepositoryBase<LikedGenre>, ILikedGenreRepository
    {
        public LikedGenreRepository(MovieDbContext movieDbContext)
            : base(movieDbContext)
        {
        }
    }
}
