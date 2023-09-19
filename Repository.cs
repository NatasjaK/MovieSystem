using System.Collections.Generic;
using System.Linq;
using DevExpress.Data.Browsing;
using Microsoft.EntityFrameworkCore;
using MovieSystem;



// Represents a generic repository interface for CRUD operations on entities.
public interface IRepository<TEntity>
{
    // Retrieves an entity by its unique identifier.
    TEntity GetById(int id);

    // Retrieves all entities of the specified type.
    IEnumerable<TEntity> GetAll();

    // Adds a new entity to the repository.
    void Add(TEntity entity);

    // Updates an existing entity in the repository.
    void Update(TEntity entity);

    // Deletes an entity from the repository by its unique identifier.
    void Delete(int id);
}

// Provides a generic implementation of the IRepository interface for Entity Framework.
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DataContext _context;

    // Constructor that accepts an instance of DataContext.
    public Repository(DataContext context)
    {
        _context = context;
    }

    // Retrieves an entity by its unique identifier.
    public TEntity GetById(int id)
    {
        return _context.Set<TEntity>().Find(id);
    }

    // Retrieves all entities of the specified type.
    public IEnumerable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().ToList();
    }

    // Adds a new entity to the repository.
    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        _context.SaveChanges();
    }

    // Updates an existing entity in the repository.
    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        _context.SaveChanges();
    }

    // Deletes an entity from the repository by its unique identifier.
    public void Delete(int id)
    {
        var entity = GetById(id);
        if (entity != null)
        {
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }
    }
}
