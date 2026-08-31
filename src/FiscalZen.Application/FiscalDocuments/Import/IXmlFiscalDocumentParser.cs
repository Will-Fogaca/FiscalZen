using FiscalZen.Domain.FiscalDocuments.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Application.FiscalDocuments.Import
{
    public interface IXmlFiscalDocumentParser
    {
        FiscalDocument Parse(string xml);
    }
}
