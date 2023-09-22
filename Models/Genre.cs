using System.ComponentModel.DataAnnotations;


namespace MovieSystem.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
      
        public string? Description { get; set; }

        public List<LikedGenre> LikedGenres { get; set; }
    }

}
