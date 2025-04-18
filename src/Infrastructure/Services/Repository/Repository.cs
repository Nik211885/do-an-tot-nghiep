﻿using Core.Interfaces;
using Core.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Services.Repository;

public class Repository<T>(ApplicationDbContext dbContext) 
    : IRepository<T> where T : IAggregateRoot
{
    /// <summary>
    /// 
    /// </summary>
    private readonly ApplicationDbContext _dbContext = dbContext;
    /// <summary>
    /// 
    /// </summary>
    public IUnitOfWork UnitOfWork => _dbContext;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    public T AddEntity(T entity)
    {
        _dbContext.Add(entity);
        return entity;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    public T UpdateEntity(T entity)
    {
        _dbContext.Update(entity);
        return entity;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    public void DeleteEntity(T entity)
      => _dbContext.Remove(entity);
}
