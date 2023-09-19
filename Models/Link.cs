using System.ComponentModel.DataAnnotations;

namespace MovieSystem.Models
{
    public class Link
    {
        [Key]
        public int Id { get; set; }
        public string? Movie { get; set; }

        public int? Rating { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }

}
