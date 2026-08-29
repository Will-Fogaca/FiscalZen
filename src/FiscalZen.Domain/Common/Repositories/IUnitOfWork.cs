using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Domain.Common.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}