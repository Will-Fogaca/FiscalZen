using FiscalZen.Domain.Common.Abstractions;
using FiscalZen.Domain.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class, IAggregateRoot
{
    protected readonly FiscalZenDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(FiscalZenDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }
}