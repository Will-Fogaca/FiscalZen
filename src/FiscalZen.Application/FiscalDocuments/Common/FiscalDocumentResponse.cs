using FiscalZen.Domain.FiscalDocuments.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Application.FiscalDocuments.Common;

public sealed record FiscalDocumentResponse
(
     Guid Id,
     string AccessKey,
     int Number,
     int Series,
     DateTime IssueDate,
     decimal ProductsAmount,
     decimal FreightAmount,
     decimal DiscountAmount,
     decimal TotalAmount,
     IReadOnlyCollection<FiscalDocumentItem> Items
);


