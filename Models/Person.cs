
using System.ComponentModel.DataAnnotations;

namespace MovieSystem.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<Genre> Genres { get; set; }
        public List<LikedGenre> LikedGenre { get; set; }
    }

}
