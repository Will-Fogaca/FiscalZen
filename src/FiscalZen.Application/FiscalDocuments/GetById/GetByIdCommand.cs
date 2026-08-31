using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Application.FiscalDocuments.GetById;

public sealed record GetByIdCommand(Guid Id, Guid AccountId);
