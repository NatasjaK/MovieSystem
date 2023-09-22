
using System.ComponentModel.DataAnnotations;

namespace MovieSystem.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        public List<LikedGenre> LikedGenres { get; set; }
    }

}
