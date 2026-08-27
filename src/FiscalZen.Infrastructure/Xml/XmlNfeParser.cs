using FiscalZen.Application.FiscalDocuments.Import;
using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System.Xml.Linq;

namespace FiscalZen.Infrastructure.Xml;

public sealed class NFeXmlParser : IXmlFiscalDocumentParser
{
    public FiscalDocument Parse(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
            throw new ArgumentException("O XML não foi informado.", nameof(xml));

        var document = XDocument.Parse(xml);

        XNamespace ns = "http://www.portalfiscal.inf.br/nfe";

        var infNFe = document.Descendants(ns + "infNFe").FirstOrDefault();

        if (infNFe is null)
            throw new InvalidOperationException("O XML informado não contém uma NF-e válida.");

        var ide = infNFe.Element(ns + "ide");

        if (ide is null)
            throw new InvalidOperationException("A identificação da NF-e não foi encontrada.");

        var accessKey = GetAccessKey(infNFe);
        var number = GetInt(ide, ns + "nNF");
        var series = GetInt(ide, ns + "serie");
        var issueDate = GetDateTime(ide, ns + "dhEmi");
        var purpose = GetPurpose(ide, ns);

        return CreateNFe(accessKey, number, series, issueDate, purpose, ide, ns);
    }

    private static AccessKey GetAccessKey(XElement infNFe)
    {
        var id = infNFe.Attribute("Id")?.Value;

        if (string.IsNullOrWhiteSpace(id))
            throw new InvalidOperationException("A chave de acesso da NF-e não foi encontrada.");

        var value = id.StartsWith("NFe") ? id[3..] : id;

        return new AccessKey(value);
    }

    private static AccessKey GetReferencedAccessKey(XElement ide, XNamespace ns)
    {
        var value = ide
            .Elements(ns + "NFref")
            .Select(x => x.Element(ns + "refNFe")?.Value)
            .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("A chave de acesso da NF-e referenciada não foi encontrada.");

        return new AccessKey(value);
    }

    private static int GetInt(XElement element, XName name)
    {
        var value = element.Element(name)?.Value;

        if (!int.TryParse(value, out var result))
            throw new InvalidOperationException($"O campo {name.LocalName} da NF-e é inválido.");

        return result;
    }

    private static DateTime GetDateTime(XElement element, XName name)
    {
        var value = element.Element(name)?.Value;

        if (!DateTime.TryParse(value, out var result))
            throw new InvalidOperationException($"O campo {name.LocalName} da NF-e é inválido.");

        return result;
    }

    private static NfePurpose GetPurpose(XElement ide, XNamespace ns)
    {
        var value = GetInt(ide, ns + "finNFe");

        return value switch
        {
            1 => NfePurpose.Normal,
            2 => NfePurpose.Complementary,
            3 => NfePurpose.Adjustment,
            4 => NfePurpose.Return,

            _ => throw new InvalidOperationException($"A finalidade da NF-e {value} não é suportada.")
        };
    }

    private static FiscalDocument CreateNFe(AccessKey accessKey, int number, int series, DateTime issueDate, NfePurpose purpose, XElement ide, XNamespace ns)
    {
        return purpose switch
        {
            NfePurpose.Normal => new NormalNfe(accessKey, number, series, issueDate, TaxRegime.LucroReal),

            NfePurpose.Complementary => throw new NotImplementedException("A leitura para a NF-e complementar ainda não está implementada."),

            NfePurpose.Adjustment => throw new NotImplementedException("A leitura para a NF-e de ajuste ainda não está implementada."),

            NfePurpose.Return => new ReturnNfe(accessKey, GetReferencedAccessKey(ide, ns), number, series, issueDate, TaxRegime.LucroReal),

            _ => throw new NotSupportedException($"A finalidade {purpose} não é suportada.")
        };
    }
}