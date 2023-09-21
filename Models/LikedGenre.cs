using System.ComponentModel.DataAnnotations;

namespace MovieSystem.Models
{
    public class LikedGenre
    {
        [Key]
        public int Id { get; set; }
        public string? Movie { get; set; }

        public decimal? Rating { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }

}
