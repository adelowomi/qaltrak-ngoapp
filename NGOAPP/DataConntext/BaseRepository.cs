using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace NGOAPP;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly Context _context;

    /// <summary>
    /// Constructor, This Constructor Sub-Method Parent Constructor To
    /// Initialize Application DbContext
    /// Also This Generic Method, Extends The Basic CRUD Stubs To Other Repository
    /// </summary>
    /// <param name="context"></param>
    public BaseRepository(Context context)
    {
        _context = context;
    }

    /// <summary>
    ///  SaveAsync Method For Saving Type T where : BaseEntity
    /// </summary>
    protected void SaveAsync()
    {
        _context.SaveChangesAsync();
    }

    /// <summary>
    /// Save Method For Entity
    /// </summary>
    protected void Save()
    {
        _context.SaveChanges();
    }

    /// <summary>
    /// Abstraction Method To Give Count Of Entitys
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public int Count(Func<T, bool> predicate)
    {
        return _context.Set<T>().Where(predicate).Count();
    }

    /// <summary>
    /// Abstraction Method To Give Count Of Entitys
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public int Count()
    {
        return _context.Set<T>().Count();
    }

    /// <summary>
    /// Method To Create Record
    /// </summary>
    /// <param name="entity"></param>
    public void Create(T entity)
    {
        _context.Add(entity);
        Save();
    }

    /// <summary>
    /// Method To Create Record And Return At The Same Time
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public T CreateAndReturn(T entity)
    {
        Create(entity);

        return entity;
    }

    /// <summary>
    /// This Method Deletes Entity 'T'
    /// </summary>
    /// <param name="entity"></param>
    public void Delete(T entity)
    {
        _context.Remove(entity);

        Save();
    }

    /// <summary>
    /// Find Entity By Argument Predicate
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public IEnumerable<T> Find(Func<T, bool> predicate)
    {
        return _context.Set<T>().Where(predicate);
    }

    /// <summary>
    /// Get All  Values For Entity
    /// </summary>
    /// <returns></returns>
    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>();
    }

    /// <summary>
    /// Get All  Values For Entity as a query
    /// </summary>
    /// <returns></returns>
    public IQueryable<T> Query()
    {
        return _context.Set<T>();
    }

    /// <summary>
    /// Get Entity By Its Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    /// <summary>
    /// Get Entity By Its Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public T GetById(Guid id)
    {
        return _context.Set<T>().Find(id);
    }

    /// <summary>
    /// Convert To List<T>
    /// </summary>
    /// <returns></returns>
    public List<T> ToList()
    {
        return _context.Set<T>().ToList<T>();
    }

    /// <summary>
    /// Update Entity Object
    /// </summary>
    /// <param name="entity"></param>
    public T Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;

        Save();

        return entity;
    }

    /// <summary>
    /// Create Multiple
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public List<T> CreateMultiple(List<T> entity)
    {
        _context.Set<T>().AddRange(entity);
        Save();
        return entity;
    }

    /// <summary>
    ///  begins a transaction
    /// </summary>
    /// <returns></returns>
    public IDbContextTransaction BeginTransaction()
    {
        return _context.Database.BeginTransaction();
    }

    /// <summary>
    /// returns true if any entity exists
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns> 
    public bool Any(Func<T, bool> value)
    {
        return _context.Set<T>().Any(value);
    }
}
