using FiscalZen.Domain.FiscalDocuments.Entities;
using FiscalZen.Domain.FiscalDocuments.Enums;
using FiscalZen.Domain.FiscalDocuments.ValueObjects;
using FiscalZen.Infrastructure.Xml;

namespace FiscalZen.Infrastructure.Tests.Xml
{
    public class NFeXmlParserTests
    {
        [Test(Description = "Deve identificar NF-e do Simples Nacional pelo CRT")]
        public void Should_Parse_Simples_Nacional_Tax_Regime()
        {
            var xml = CreateValidXml(1);

            var parser = new NFeXmlParser();

            var result = parser.Parse(xml);

            Assert.That(result.TaxRegime, Is.EqualTo(TaxRegime.SimplesNacional));
        }

        [Test(Description = "Deve identificar NF-e do Regime Normal pelo CRT")]
        public void Should_Parse_Normal_Tax_Regime()
        {
            var xml = CreateValidXml(3);

            var parser = new NFeXmlParser();

            var result = parser.Parse(xml);

            Assert.That(result.TaxRegime, Is.EqualTo(TaxRegime.RegimeNormal));
        }

        [Test(Description = "Deve ler os valores totais da NF-e")]
        public void Should_Parse_NFe_Totals()
        {
            var xml = CreateValidXml(3);

            var parser = new NFeXmlParser();

            var result = parser.Parse(xml);

            Assert.Multiple(() =>
            {
                Assert.That(result.ProductsAmount.Value, Is.EqualTo(100m));
                Assert.That(result.FreightAmount.Value, Is.EqualTo(10m));
                Assert.That(result.DiscountAmount.Value, Is.EqualTo(5m));
                Assert.That(result.TotalAmount.Value, Is.EqualTo(105m));
            });
        }

        [Test(Description = "Deve adicionar os itens da NF-e")]
        public void Should_Parse_NFe_Items()
        {
            var xml = CreateValidXml(3);

            var parser = new NFeXmlParser();

            var result = parser.Parse(xml);

            Assert.That(result.Items.Count, Is.EqualTo(1));

            var item = result.Items.First();

            Assert.Multiple(() =>
            {
                Assert.That(item.Number, Is.EqualTo(1));
                Assert.That(item.ProductCode, Is.EqualTo("PROD001"));
                Assert.That(item.Description, Is.EqualTo("Produto teste"));
                Assert.That(item.Ncm, Is.EqualTo(new Ncm("12345678")));
                Assert.That(item.Cfop.Value, Is.EqualTo("5102"));
                Assert.That(item.Quantity, Is.EqualTo(2m));
                Assert.That(item.UnitPrice.Value, Is.EqualTo(50m));
                Assert.That(item.TotalAmount.Value, Is.EqualTo(100m));
            });
        }

        [Test(Description = "Não deve permitir NF-e sem dados do emitente")]
        public void Should_Throw_When_Issuer_Is_Not_Found()
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

            Assert.Throws<InvalidOperationException>(() => parser.Parse(xml));
        }

        [Test(Description = "Não deve permitir NF-e sem CRT do emitente")]
        public void Should_Throw_When_CRT_Is_Not_Found()
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
                            <emit>
                            </emit>
                        </infNFe>
                    </NFe>
                </nfeProc>
                """;

            var parser = new NFeXmlParser();

            Assert.Throws<InvalidOperationException>(() => parser.Parse(xml));
        }

        [Test(Description = "Não deve permitir CRT não suportado")]
        public void Should_Throw_When_CRT_Is_Not_Supported()
        {
            var xml = CreateValidXml(99);

            var parser = new NFeXmlParser();

            Assert.Throws<InvalidOperationException>(() => parser.Parse(xml));
        }

        [Test(Description = "Não deve permitir NF-e sem totais")]
        public void Should_Throw_When_Totals_Are_Not_Found()
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
                            <emit>
                                <CRT>3</CRT>
                            </emit>
                        </infNFe>
                    </NFe>
                </nfeProc>
                """;

            var parser = new NFeXmlParser();

            Assert.Throws<InvalidOperationException>(() => parser.Parse(xml));
        }

        [Test(Description = "Não deve permitir item com número inválido")]
        public void Should_Throw_When_Item_Number_Is_Invalid()
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

                            <emit>
                                <CRT>3</CRT>
                            </emit>

                            <det nItem="ABC">
                                <prod>
                                    <cProd>PROD001</cProd>
                                    <xProd>Produto teste</xProd>
                                    <NCM>12345678</NCM>
                                    <CFOP>5102</CFOP>
                                    <qCom>2.0000</qCom>
                                    <vUnCom>50.00</vUnCom>
                                    <vProd>100.00</vProd>
                                </prod>
                            </det>

                            <total>
                                <ICMSTot>
                                    <vProd>100.00</vProd>
                                    <vFrete>10.00</vFrete>
                                    <vDesc>5.00</vDesc>
                                    <vNF>105.00</vNF>
                                </ICMSTot>
                            </total>
                        </infNFe>
                    </NFe>
                </nfeProc>
                """;

            var parser = new NFeXmlParser();

            Assert.Throws<InvalidOperationException>(() => parser.Parse(xml));
        }

        [Test(Description = "Não deve permitir item sem dados do produto")]
        public void Should_Throw_When_Product_Is_Not_Found()
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

                            <emit>
                                <CRT>3</CRT>
                            </emit>

                            <det nItem="1">
                            </det>

                            <total>
                                <ICMSTot>
                                    <vProd>100.00</vProd>
                                    <vFrete>10.00</vFrete>
                                    <vDesc>5.00</vDesc>
                                    <vNF>105.00</vNF>
                                </ICMSTot>
                            </total>
                        </infNFe>
                    </NFe>
                </nfeProc>
                """;

            var parser = new NFeXmlParser();

            Assert.Throws<InvalidOperationException>(() => parser.Parse(xml));
        }

        private static string CreateValidXml(int crt)
        {
            return $$"""
                <nfeProc xmlns="http://www.portalfiscal.inf.br/nfe">
                    <NFe>
                        <infNFe Id="NFe35260812345678000190550010000012341000012345">
                            <ide>
                                <serie>1</serie>
                                <nNF>123</nNF>
                                <dhEmi>2026-08-26T20:00:00-03:00</dhEmi>
                                <finNFe>1</finNFe>
                            </ide>

                            <emit>
                                <CRT>{{crt}}</CRT>
                            </emit>

                            <det nItem="1">
                                <prod>
                                    <cProd>PROD001</cProd>
                                    <xProd>Produto teste</xProd>
                                    <NCM>12345678</NCM>
                                    <CFOP>5102</CFOP>
                                    <qCom>2.0000</qCom>
                                    <vUnCom>50.00</vUnCom>
                                    <vProd>100.00</vProd>
                                </prod>
                            </det>

                            <total>
                                <ICMSTot>
                                    <vProd>100.00</vProd>
                                    <vFrete>10.00</vFrete>
                                    <vDesc>5.00</vDesc>
                                    <vNF>105.00</vNF>
                                </ICMSTot>
                            </total>
                        </infNFe>
                    </NFe>
                </nfeProc>
                """;
        }

        [Test(Description = "Deve converter uma NF-e completa com itens e tributos")]
        public void Should_Parse_Complete_NFe()
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

                            <emit>
                                <CRT>3</CRT>
                            </emit>

                            <det nItem="1">
                                <prod>
                                    <cProd>PROD001</cProd>
                                    <xProd>Produto teste 1</xProd>
                                    <NCM>12345678</NCM>
                                    <CFOP>5102</CFOP>
                                    <qCom>2.0000</qCom>
                                    <vUnCom>50.00</vUnCom>
                                    <vProd>100.00</vProd>
                                </prod>

                                <imposto>
                                    <ICMS>
                                        <ICMS00>
                                            <vICMS>18.00</vICMS>
                                        </ICMS00>
                                    </ICMS>

                                    <IPI>
                                        <IPITrib>
                                            <vIPI>10.00</vIPI>
                                        </IPITrib>
                                    </IPI>

                                    <PIS>
                                        <PISAliq>
                                            <vPIS>1.65</vPIS>
                                        </PISAliq>
                                    </PIS>

                                    <COFINS>
                                        <COFINSAliq>
                                            <vCOFINS>7.60</vCOFINS>
                                        </COFINSAliq>
                                    </COFINS>

                                    <IBSCBS>
                                        <gIBS>
                                            <vIBS>5.00</vIBS>
                                        </gIBS>

                                        <gCBS>
                                            <vCBS>8.00</vCBS>
                                        </gCBS>
                                    </IBSCBS>
                                </imposto>
                            </det>

                            <det nItem="2">
                                <prod>
                                    <cProd>PROD002</cProd>
                                    <xProd>Produto teste 2</xProd>
                                    <NCM>87654321</NCM>
                                    <CFOP>5102</CFOP>
                                    <qCom>1.0000</qCom>
                                    <vUnCom>200.00</vUnCom>
                                    <vProd>200.00</vProd>
                                </prod>

                                <imposto>
                                    <ICMS>
                                        <ICMS00>
                                            <vICMS>36.00</vICMS>
                                        </ICMS00>
                                    </ICMS>

                                    <IPI>
                                        <IPITrib>
                                            <vIPI>20.00</vIPI>
                                        </IPITrib>
                                    </IPI>

                                    <PIS>
                                        <PISAliq>
                                            <vPIS>3.30</vPIS>
                                        </PISAliq>
                                    </PIS>

                                    <COFINS>
                                        <COFINSAliq>
                                            <vCOFINS>15.20</vCOFINS>
                                        </COFINSAliq>
                                    </COFINS>

                                    <IBSCBS>
                                        <gIBS>
                                            <vIBS>10.00</vIBS>
                                        </gIBS>

                                        <gCBS>
                                            <vCBS>16.00</vCBS>
                                        </gCBS>
                                    </IBSCBS>
                                </imposto>
                            </det>

                            <total>
                                <ICMSTot>
                                    <vProd>300.00</vProd>
                                    <vFrete>20.00</vFrete>
                                    <vDesc>10.00</vDesc>
                                    <vNF>310.00</vNF>

                                    <vICMS>54.00</vICMS>
                                    <vIPI>30.00</vIPI>
                                    <vPIS>4.95</vPIS>
                                    <vCOFINS>22.80</vCOFINS>
                                </ICMSTot>

                                <IBSCBSTot>
                                    <gIBS>
                                        <vIBS>15.00</vIBS>
                                    </gIBS>

                                    <gCBS>
                                        <vCBS>24.00</vCBS>
                                    </gCBS>
                                </IBSCBSTot>
                            </total>
                        </infNFe>
                    </NFe>
                </nfeProc>
                """;

            var parser = new NFeXmlParser();

            var result = parser.Parse(xml);

            Assert.That(result, Is.TypeOf<NormalNfe>());

            var nfe = (NormalNfe)result;

            Assert.Multiple(() =>
            {
                Assert.That(nfe.AccessKey.Value, Is.EqualTo("35260812345678000190550010000012341000012345"));
                Assert.That(nfe.Number, Is.EqualTo(123));
                Assert.That(nfe.Series, Is.EqualTo(1));
                Assert.That(nfe.Purpose, Is.EqualTo(NfePurpose.Normal));
                Assert.That(nfe.TaxRegime, Is.EqualTo(TaxRegime.RegimeNormal));

                Assert.That(nfe.ProductsAmount.Value, Is.EqualTo(300m));
                Assert.That(nfe.FreightAmount.Value, Is.EqualTo(20m));
                Assert.That(nfe.DiscountAmount.Value, Is.EqualTo(10m));
                Assert.That(nfe.TotalAmount.Value, Is.EqualTo(310m));

                Assert.That(nfe.Taxes.ICMS.Value, Is.EqualTo(54m));
                Assert.That(nfe.Taxes.IPI.Value, Is.EqualTo(30m));
                Assert.That(nfe.Taxes.PIS.Value, Is.EqualTo(4.95m));
                Assert.That(nfe.Taxes.COFINS.Value, Is.EqualTo(22.80m));
                Assert.That(nfe.Taxes.IBS.Value, Is.EqualTo(15m));
                Assert.That(nfe.Taxes.CBS.Value, Is.EqualTo(24m));

                Assert.That(nfe.Items.Count, Is.EqualTo(2));
            });

            var firstItem = nfe.Items.First(x => x.Number == 1);

            Assert.Multiple(() =>
            {
                Assert.That(firstItem.ProductCode, Is.EqualTo("PROD001"));
                Assert.That(firstItem.Description, Is.EqualTo("Produto teste 1"));
                Assert.That(firstItem.Ncm, Is.EqualTo(new Ncm("12345678")));
                Assert.That(firstItem.Cfop.Value, Is.EqualTo("5102"));
                Assert.That(firstItem.Quantity, Is.EqualTo(2m));
                Assert.That(firstItem.UnitPrice.Value, Is.EqualTo(50m));
                Assert.That(firstItem.TotalAmount.Value, Is.EqualTo(100m));

                Assert.That(firstItem.Taxes.ICMS.Value, Is.EqualTo(18m));
                Assert.That(firstItem.Taxes.IPI.Value, Is.EqualTo(10m));
                Assert.That(firstItem.Taxes.PIS.Value, Is.EqualTo(1.65m));
                Assert.That(firstItem.Taxes.COFINS.Value, Is.EqualTo(7.60m));
                Assert.That(firstItem.Taxes.IBS.Value, Is.EqualTo(5m));
                Assert.That(firstItem.Taxes.CBS.Value, Is.EqualTo(8m));
            });

            var secondItem = nfe.Items.First(x => x.Number == 2);

            Assert.Multiple(() =>
            {
                Assert.That(secondItem.ProductCode, Is.EqualTo("PROD002"));
                Assert.That(secondItem.Description, Is.EqualTo("Produto teste 2"));
                Assert.That(secondItem.Ncm, Is.EqualTo(new Ncm("87654321")));
                Assert.That(secondItem.Cfop.Value, Is.EqualTo("5102"));
                Assert.That(secondItem.Quantity, Is.EqualTo(1m));
                Assert.That(secondItem.UnitPrice.Value, Is.EqualTo(200m));
                Assert.That(secondItem.TotalAmount.Value, Is.EqualTo(200m));

                Assert.That(secondItem.Taxes.ICMS.Value, Is.EqualTo(36m));
                Assert.That(secondItem.Taxes.IPI.Value, Is.EqualTo(20m));
                Assert.That(secondItem.Taxes.PIS.Value, Is.EqualTo(3.30m));
                Assert.That(secondItem.Taxes.COFINS.Value, Is.EqualTo(15.20m));
                Assert.That(secondItem.Taxes.IBS.Value, Is.EqualTo(10m));
                Assert.That(secondItem.Taxes.CBS.Value, Is.EqualTo(16m));
            });
        }
   

    [Test(Description = "Deve iniciar os tributos do item zerados quando o XML não informar impostos")]
        public void Should_Set_Zero_Taxes_When_Item_Has_No_Tax_Element()
        {
            var xml = CreateValidXml(3);

            var parser = new NFeXmlParser();

            var result = parser.Parse(xml);

            var item = result.Items.First();

            Assert.Multiple(() =>
            {
                Assert.That(item.Taxes.ICMS, Is.EqualTo(Money.Zero));
                Assert.That(item.Taxes.IPI, Is.EqualTo(Money.Zero));
                Assert.That(item.Taxes.PIS, Is.EqualTo(Money.Zero));
                Assert.That(item.Taxes.COFINS, Is.EqualTo(Money.Zero));
                Assert.That(item.Taxes.IBS, Is.EqualTo(Money.Zero));
                Assert.That(item.Taxes.CBS, Is.EqualTo(Money.Zero));
            });
        }
    }
}