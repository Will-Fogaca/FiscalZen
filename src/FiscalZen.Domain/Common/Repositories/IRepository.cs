using FiscalZen.Domain.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.Common.Repositories;

public interface IRepository<T> where T : class, IAggregateRoot
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    void Update(T entity);

    void Remove(T entity);
}