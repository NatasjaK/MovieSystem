using MovieSystem.Models;

namespace MovieSystem.RepositoryPattern
{
    // Define a public class named "PersonRepository" that inherits from "RepositoryBase<Person>" and implements "IPersonRepository"
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        // Constructor for the "PersonRepository" class, which takes an instance of "MovieDbContext" as a parameter
        public PersonRepository(MovieDbContext repositoryContext)
            : base(repositoryContext) // Call the base class constructor with the "repositoryContext" parameter
        {
        }
    }

}
