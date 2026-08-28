using FiscalZen.Application.FiscalDocuments.Import;
using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using System.Globalization;
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
        var taxRegime = GetTaxRegime(infNFe, ns);

        var fiscalDocument = CreateNFe(
            accessKey,
            number,
            series,
            issueDate,
            purpose,
            taxRegime,
            ide,
            ns);

        SetTotals(fiscalDocument, infNFe, ns);

        SetDocumentTaxes(fiscalDocument, infNFe, ns);

        SetItems(fiscalDocument, infNFe, ns);

        return fiscalDocument;
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

    private static string GetString(XElement element, XName name)
    {
        var value = element.Element(name)?.Value;

        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"O campo {name.LocalName} da NF-e não foi encontrado.");

        return value;
    }

    private static int GetInt(XElement element, XName name)
    {
        var value = element.Element(name)?.Value;

        if (!int.TryParse(value, out var result))
            throw new InvalidOperationException($"O campo {name.LocalName} da NF-e é inválido.");

        return result;
    }

    private static decimal GetDecimal(XElement element, XName name)
    {
        var value = element.Element(name)?.Value;

        if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
            throw new InvalidOperationException($"O campo {name.LocalName} da NF-e é inválido.");

        return result;
    }

    private static decimal GetOptionalDecimal(XElement? element, XName name)
    {
        if (element is null)
            return 0;

        var value = element.Element(name)?.Value;

        if (string.IsNullOrWhiteSpace(value))
            return 0;

        if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
            throw new InvalidOperationException($"O campo {name.LocalName} da NF-e é inválido.");

        return result;
    }

    private static decimal GetDescendantDecimal(XElement? element, string fieldName)
    {
        if (element is null)
            return 0;

        var value = element
            .Descendants()
            .FirstOrDefault(x => x.Name.LocalName == fieldName)?
            .Value;

        if (string.IsNullOrWhiteSpace(value))
            return 0;

        if (!decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
            throw new InvalidOperationException($"O campo {fieldName} da NF-e é inválido.");

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

    private static TaxRegime GetTaxRegime(XElement infNFe, XNamespace ns)
    {
        var emit = infNFe.Element(ns + "emit");

        if (emit is null)
            throw new InvalidOperationException("Os dados do emitente da NF-e não foram encontrados.");

        var crt = GetInt(emit, ns + "CRT");

        return crt switch
        {
            1 => TaxRegime.SimplesNacional,
            2 => TaxRegime.SimplesNacional,
            3 => TaxRegime.RegimeNormal,

            _ => throw new InvalidOperationException($"O regime tributário CRT {crt} não é suportado.")
        };
    }

    private static void SetTotals(FiscalDocument fiscalDocument, XElement infNFe, XNamespace ns)
    {
        var total = infNFe.Element(ns + "total");

        if (total is null)
            throw new InvalidOperationException("Os totais da NF-e não foram encontrados.");

        var icmsTot = total.Element(ns + "ICMSTot");

        if (icmsTot is null)
            throw new InvalidOperationException("Os totais da NF-e não foram encontrados.");

        var productsAmount = GetDecimal(icmsTot, ns + "vProd");
        var freightAmount = GetOptionalDecimal(icmsTot, ns + "vFrete");
        var discountAmount = GetOptionalDecimal(icmsTot, ns + "vDesc");
        var totalAmount = GetDecimal(icmsTot, ns + "vNF");

        fiscalDocument.SetProductsAmount(new Money(productsAmount));
        fiscalDocument.SetFreightAmount(new Money(freightAmount));
        fiscalDocument.SetDiscountAmount(new Money(discountAmount));
        fiscalDocument.SetTotalAmount(new Money(totalAmount));
    }

    private static void SetDocumentTaxes(FiscalDocument fiscalDocument, XElement infNFe, XNamespace ns)
    {
        var total = infNFe.Element(ns + "total");

        if (total is null)
            throw new InvalidOperationException("Os totais da NF-e não foram encontrados.");

        var icmsTot = total.Element(ns + "ICMSTot");
        var ibsCbsTot = total.Element(ns + "IBSCBSTot");

        var icms = GetOptionalDecimal(icmsTot, ns + "vICMS");
        var ipi = GetOptionalDecimal(icmsTot, ns + "vIPI");
        var pis = GetOptionalDecimal(icmsTot, ns + "vPIS");
        var cofins = GetOptionalDecimal(icmsTot, ns + "vCOFINS");

        var ibs = GetDescendantDecimal(ibsCbsTot, "vIBS");
        var cbs = GetDescendantDecimal(ibsCbsTot, "vCBS");

        var taxes = new TaxSummary
        {
            ICMS = new Money(icms),
            IPI = new Money(ipi),
            PIS = new Money(pis),
            COFINS = new Money(cofins),
            IBS = new Money(ibs),
            CBS = new Money(cbs)
        };

        fiscalDocument.SetTaxes(taxes);
    }

    private static void SetItems(FiscalDocument fiscalDocument, XElement infNFe, XNamespace ns)
    {
        var items = infNFe.Elements(ns + "det");

        foreach (var itemElement in items)
        {
            var numberValue = itemElement.Attribute("nItem")?.Value;

            if (!int.TryParse(numberValue, out var number))
                throw new InvalidOperationException("O número do item da NF-e é inválido.");

            var product = itemElement.Element(ns + "prod");

            if (product is null)
                throw new InvalidOperationException($"Os dados do item {number} da NF-e não foram encontrados.");

            var productCode = GetString(product, ns + "cProd");
            var description = GetString(product, ns + "xProd");
            var ncm = new Ncm(GetString(product, ns + "NCM"));
            var cfop = new Cfop(GetString(product, ns + "CFOP"));
            var quantity = GetDecimal(product, ns + "qCom");
            var unitPrice = GetDecimal(product, ns + "vUnCom");
            var totalAmount = GetDecimal(product, ns + "vProd");

            var item = new FiscalDocumentItem(
                number,
                productCode,
                description,
                ncm,
                cfop,
                quantity,
                new Money(unitPrice),
                new Money(totalAmount));

            SetItemTaxes(item, itemElement, ns);

            fiscalDocument.AddItem(item);
        }
    }

    private static void SetItemTaxes(FiscalDocumentItem item, XElement itemElement, XNamespace ns)
    {
        var taxElement = itemElement.Element(ns + "imposto");

        if (taxElement is null)
        {
            item.SetTaxes(new TaxSummary());

            return;
        }

        var icmsGroup = taxElement.Element(ns + "ICMS");
        var ipiGroup = taxElement.Element(ns + "IPI");
        var pisGroup = taxElement.Element(ns + "PIS");
        var cofinsGroup = taxElement.Element(ns + "COFINS");
        var ibsCbsGroup = taxElement.Element(ns + "IBSCBS");

        var icms = GetDescendantDecimal(icmsGroup, "vICMS");
        var ipi = GetDescendantDecimal(ipiGroup, "vIPI");
        var pis = GetDescendantDecimal(pisGroup, "vPIS");
        var cofins = GetDescendantDecimal(cofinsGroup, "vCOFINS");
        var ibs = GetDescendantDecimal(ibsCbsGroup, "vIBS");
        var cbs = GetDescendantDecimal(ibsCbsGroup, "vCBS");

        var taxes = new TaxSummary
        {
            ICMS = new Money(icms),
            IPI = new Money(ipi),
            PIS = new Money(pis),
            COFINS = new Money(cofins),
            IBS = new Money(ibs),
            CBS = new Money(cbs)
        };

        item.SetTaxes(taxes);
    }

    private static FiscalDocument CreateNFe(AccessKey accessKey, int number, int series, DateTime issueDate, NfePurpose purpose, TaxRegime taxRegime, XElement ide, XNamespace ns)
    {
        return purpose switch
        {
            NfePurpose.Normal => new NormalNfe(
                accessKey,
                number,
                series,
                issueDate,
                taxRegime),

            NfePurpose.Complementary => throw new NotImplementedException(
                "A leitura para a NF-e complementar ainda não está implementada."),

            NfePurpose.Adjustment => throw new NotImplementedException(
                "A leitura para a NF-e de ajuste ainda não está implementada."),

            NfePurpose.Return => new ReturnNfe(
                accessKey,
                GetReferencedAccessKey(ide, ns),
                number,
                series,
                issueDate,
                taxRegime),

            _ => throw new NotSupportedException($"A finalidade {purpose} não é suportada.")
        };
    }
}