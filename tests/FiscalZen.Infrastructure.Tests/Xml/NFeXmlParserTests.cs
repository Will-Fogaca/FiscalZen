using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Infrastructure.Xml;

namespace FiscalZen.Infrastructure.Tests.Xml
{
    public class NFeXmlParserTests
    {
        [Test(Description = "Deve converter um XML de NF-e normal em NormalNFe")]
        public void Should_Parse_Normal_NFe()
        {
            var xml = """
                <nfeProc xmlns="http://www.portalfiscal.inf.br/nfe">
                    <NFe>
                        <infNFe Id="NFe35260812345678000190550010000012341000012345">
                            <ide>
                                <serie>1</serie>
                                <nNF>123</nNF>
                                <dhEmi>2026-08-26T20:00:00-03:00</dhEmi>
                                <finNFe>1</finNFe>
                            </ide>
                        </infNFe>
                    </NFe>
                </nfeProc>
                """;

            var parser = new NFeXmlParser();

            var result = parser.Parse(xml);

            var nfe = result as NormalNfe;

            Assert.That(nfe, Is.TypeOf<NormalNfe>());

            Assert.Multiple(() =>
            {
                Assert.That(nfe.AccessKey.Value, Is.EqualTo("35260812345678000190550010000012341000012345"));
                Assert.That(nfe.Number, Is.EqualTo(123));
                Assert.That(nfe.Series, Is.EqualTo(1));
                Assert.That(nfe.Purpose, Is.EqualTo(NfePurpose.Normal));
                Assert.That(nfe.TaxRegime, Is.EqualTo(TaxRegime.LucroReal));
            });
        }
    }
}