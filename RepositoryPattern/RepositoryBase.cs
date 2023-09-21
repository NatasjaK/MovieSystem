using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MovieSystem.RepositoryPattern
{
    // This is an abstract base class for repositories, providing common CRUD operations.
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        // The DbContext used for interacting with the Movie System database.
        protected MovieDbContext MovieDbContext { get; set; }

        // Constructor to initialize the DbContext.
        public RepositoryBase(MovieDbContext movieDbContext)
        {
            MovieDbContext = movieDbContext;
        }

        public IQueryable<T> GetAll() => MovieDbContext.Set<T>().AsNoTracking();

        // The 'expression' parameter allows specifying a condition using a lambda expression.
        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression) =>
            MovieDbContext.Set<T>().Where(expression).AsNoTracking();

        public void Create(T entity) => MovieDbContext.Set<T>().Add(entity);

        public void Update(T entity) => MovieDbContext.Set<T>().Update(entity);

        public void Delete(T entity) => MovieDbContext.Set<T>().Remove(entity);
    }

}
