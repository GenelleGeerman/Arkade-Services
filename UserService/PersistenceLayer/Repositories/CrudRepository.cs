using BusinessLayer.Interfaces;

namespace PersistenceLayer.Repositories;

using Microsoft.EntityFrameworkCore;

public class CrudRepository<T> : ICrudRepository<T> where T : class
{
    protected DbContext Context { get; }

    public CrudRepository(DbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public bool Create(T entity)
    {
        Context.Set<T>().Add(entity);
        return Context.SaveChanges() == 1;
    }

    public T GetById(object id)
    {
        return Context.Set<T>().Find(id);
        
    }

    public List<T> GetAll()
    {
        return Context.Set<T>().ToList();
    }

    public T Update(T entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
        return entity;
    }

    public bool Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
        return Context.SaveChanges() > 0;
    }

    public bool DeleteAll()
    {
        var list = Context.Set<T>().ToList();
        int count = list.Count;
        Context.Set<T>().RemoveRange(GetAll());
        return Context.SaveChanges() == count;
    }
}
