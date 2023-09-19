namespace MovieSystem.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<Genre> LikedGenres { get; set; }
        public List<Link> Links { get; set; }
    }

}
