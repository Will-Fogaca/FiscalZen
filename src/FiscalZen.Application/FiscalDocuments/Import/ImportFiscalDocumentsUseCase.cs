using FiscalZen.Domain.FiscalDocuments.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FiscalZen.Application.FiscalDocuments.Import
{
    public sealed class ImportFiscalDocumentsUseCase
    {
        private readonly IXmlFiscalDocumentParser _parser;

        public ImportFiscalDocumentsUseCase(IXmlFiscalDocumentParser parser)
        {
            _parser = parser;
        }

        public IEnumerable<FiscalDocument> Execute(IEnumerable<string> xmls)
        {
            foreach (var xml in xmls)
                yield return _parser.Parse(xml);
        }
    }
}
