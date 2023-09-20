using System.ComponentModel.DataAnnotations;


namespace MovieSystem.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<Genre> LikedGenres { get; set; }
    }

}
