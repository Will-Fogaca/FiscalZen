# Domain Model

## Visão geral

O domínio do FiscalZen é responsável por representar os conceitos e regras relacionados aos documentos fiscais utilizados pela aplicação.

Neste primeiro momento, o sistema possui um módulo principal:

```text
FiscalDocuments
```

Esse módulo concentra os conceitos relacionados às NF-e e NFC-e analisadas pelo sistema.

A estrutura foi organizada de forma que novos módulos ou Bounded Contexts possam ser extraídos futuramente, caso o crescimento do sistema justifique essa separação.

## Fiscal Documents

O módulo `FiscalDocuments` é responsável pela representação dos documentos fiscais eletrônicos.

Inicialmente são suportados:

* NF-e — Nota Fiscal Eletrônica, modelo 55;
* NFC-e — Nota Fiscal de Consumidor Eletrônica, modelo 65.

Ambos compartilham características comuns através da classe base `FiscalDocument`.

```text
FiscalDocument
        ▲
        │
   ┌────┴────┐
   │         │
  Nfe       Nfce
```

`FiscalDocument` concentra os dados e comportamentos comuns aos documentos fiscais.

`Nfe` e `Nfce` representam as particularidades de cada modelo.

A NF-e possui ainda finalidades que podem alterar seu comportamento dentro do domínio.

```text
FiscalDocument
├── Nfce
└── Nfe
    ├── NormalNfe
    ├── ReturnNfe
    ├── CreditNfe
    └── DebitNfe
```

Essas especializações existem porque determinadas finalidades da NF-e podem possuir regras e interpretações diferentes durante as análises fiscais.

## Principais conceitos

### FiscalDocument

Representa a base de um documento fiscal eletrônico.

Responsabilidades atuais:

* possuir uma chave de acesso;
* possuir número e série;
* informar a data de emissão;
* armazenar valores monetários;
* armazenar o regime tributário conhecido pelo sistema;
* armazenar os tributos informados no documento;
* controlar os itens do documento;
* garantir regras básicas de consistência.

As regras comuns entre NF-e e NFC-e devem permanecer nessa classe.

### Aggregate Root

`FiscalDocument` é a raiz do agregado de documentos fiscais.

```text
FiscalDocument ← Aggregate Root
│
└── FiscalDocumentItem
```

A raiz do agregado é responsável por manter a consistência do conjunto de objetos relacionados ao documento.

Por esse motivo, itens não devem ser adicionados diretamente à coleção.

A inclusão ocorre através de:

```text
FiscalDocument.AddItem(...)
```

Isso permite que regras como número de item duplicado sejam garantidas pelo domínio.

`FiscalDocumentItem` é uma Entity pertencente ao agregado, mas não é uma Aggregate Root.

A estrutura do agregado não determina a estrutura física do banco.

O agregado poderá ser persistido em tabelas distintas, como:

```text
FiscalDocuments
FiscalDocumentItems
```

sem alterar sua fronteira no domínio.

### Nfe

Representa uma Nota Fiscal Eletrônica modelo 55.

Herda as características comuns de `FiscalDocument` e concentra comportamentos e informações específicos da NF-e.

A NF-e possui diferentes finalidades que podem alterar a forma como o documento deve ser interpretado pelo sistema.

### NormalNfe

Representa uma NF-e de finalidade normal.

É utilizada para operações comuns realizadas através da NF-e, respeitando as regras específicas da operação e da tributação informada no documento.

### ReturnNfe

Representa uma NF-e de devolução.

Uma devolução possui significado diferente de uma operação normal e seus valores não devem ser tratados automaticamente como novos valores de faturamento ou tributação.

A interpretação dos valores de uma devolução deve respeitar as regras fiscais aplicáveis ao documento e aos relatórios gerados pelo sistema.

### CreditNfe

Representa uma NF-e de crédito.

Além da finalidade de crédito, esse documento pode possuir um tipo específico representado pelo enum `NfeCreditType`.

O tipo de crédito poderá influenciar regras fiscais e análises realizadas pelo sistema.

### DebitNfe

Representa uma NF-e de débito.

Além da finalidade de débito, esse documento pode possuir um tipo específico representado pelo enum `NfeDebitType`.

O tipo de débito poderá influenciar regras fiscais e análises realizadas pelo sistema.

### Nfce

Representa uma Nota Fiscal de Consumidor Eletrônica modelo 65.

Herda as características comuns de `FiscalDocument`, porém possui características e regras próprias relacionadas à NFC-e.

Ela não participa da mesma hierarquia de finalidades específicas da NF-e.

### FiscalDocumentItem

Representa um item existente dentro de um documento fiscal.

Pode conter informações como:

* código do produto;
* descrição;
* NCM;
* CFOP;
* quantidade;
* valor unitário;
* valor total;
* tributos relacionados ao item.

As regras específicas dos itens devem permanecer próximas a esse conceito quando fizerem parte do domínio.

## Value Objects

### AccessKey

`AccessKey` é um Value Object que representa a chave de acesso de um documento fiscal.

Regras iniciais:

* deve ser informada;
* deve possuir 44 dígitos;
* deve conter apenas números;
* deve ser tratada como um valor imutável.

A utilização de um Value Object evita representar uma chave de acesso simplesmente como uma `string`, permitindo que suas regras sejam garantidas pelo próprio domínio.

### Money

`Money` é um Value Object utilizado para representar valores monetários dentro do domínio.

Exemplos:

* valor dos produtos;
* frete;
* descontos;
* valor total;
* valores tributários.

O uso de `Money` evita representar conceitos monetários diretamente através de `decimal` em todo o domínio.

O próprio Value Object fornece comportamentos relacionados a valores monetários, como soma, subtração, multiplicação, divisão e comparação.

### Ncm

`Ncm` representa a Nomenclatura Comum do Mercosul associada ao item do documento fiscal.

Regras iniciais:

* deve ser informado;
* deve possuir 8 dígitos;
* deve conter apenas números.

O NCM possui significado e regras próprias e, por isso, não é representado apenas como `string`.

### Cfop

`Cfop` representa o Código Fiscal de Operações e Prestações.

Regras iniciais:

* deve ser informado;
* deve possuir 4 dígitos;
* deve conter apenas números.

A validação estrutural do código não representa necessariamente a validação da existência do CFOP na tabela fiscal oficial.

### TaxSummary

`TaxSummary` representa um conjunto de valores tributários informados em um documento fiscal ou item.

Atualmente contempla:

* ICMS;
* IPI;
* PIS;
* COFINS;
* IBS;
* CBS.

Os valores representam os dados originais do documento.

`TaxSummary` não determina sozinho como esses valores devem participar de uma apuração ou relatório.

## Enums

### NfePurpose

Representa a finalidade de uma NF-e.

As finalidades fazem parte do significado do documento e podem alterar a forma como seus valores são interpretados pelo sistema.

Exemplos:

* Normal;
* Complementary;
* Adjustment;
* Return;
* Credit;
* Debit.

Sempre que uma finalidade possuir comportamento próprio relevante para o domínio, poderá existir uma especialização de `Nfe` correspondente.

### NfeCreditType

Representa o tipo específico de uma NF-e com finalidade de crédito.

Esse enum utiliza os códigos definidos para o documento fiscal.

Descrições em PT-BR podem ser associadas aos valores para utilização em relatórios e interfaces.

### NfeDebitType

Representa o tipo específico de uma NF-e com finalidade de débito.

Assim como `NfeCreditType`, os valores representam códigos fiscais e podem possuir descrições em PT-BR para apresentação ao usuário.

### TaxRegime

`TaxRegime` representa a classificação tributária conhecida pelo sistema.

O parser pode obter informações de regime disponíveis no XML, como o CRT.

Quando o XML não permite distinguir completamente regimes como Lucro Real e Lucro Presumido, essa informação deverá ser complementada por outra fonte do sistema.

## Apuração tributária

O FiscalZen possui regras específicas para interpretar os valores tributários presentes nos documentos.

A leitura dos valores e a apuração são responsabilidades diferentes.

```text
XML
 ↓
TaxSummary
 ↓
Tax Assessment Rules
 ↓
Resultado da apuração
```

Atualmente a estrutura de apuração utiliza regras específicas por tipo de documento.

Exemplo:

```text
LucroRealTaxAssessmentService
│
├── NormalNfeTaxAssessmentRule
├── ReturnNfeTaxAssessmentRule
├── CreditNfeTaxAssessmentRule
└── DebitNfeTaxAssessmentRule
```

O serviço de apuração coordena as regras, enquanto cada regra é responsável por interpretar um tipo específico de documento.

Esse desenho evita concentrar todas as regras fiscais em uma única classe e permite adicionar novos comportamentos sem modificar a lógica já existente.

## Linguagem ubíqua

| Termo no código | Significado no domínio |
| --- | --- |
| FiscalDocument | Documento fiscal eletrônico |
| Nfe | Nota Fiscal Eletrônica modelo 55 |
| Nfce | Nota Fiscal de Consumidor Eletrônica modelo 65 |
| NormalNfe | NF-e de finalidade normal |
| ReturnNfe | NF-e de devolução |
| CreditNfe | NF-e de crédito |
| DebitNfe | NF-e de débito |
| NfePurpose | Finalidade da NF-e |
| NfeCreditType | Tipo específico de NF-e de crédito |
| NfeDebitType | Tipo específico de NF-e de débito |
| FiscalDocumentItem | Item do documento fiscal |
| AccessKey | Chave de acesso do documento fiscal |
| Money | Valor monetário |
| Ncm | Nomenclatura Comum do Mercosul |
| Cfop | Código Fiscal de Operações e Prestações |
| TaxSummary | Resumo dos valores tributários |
| TaxRegime | Regime tributário conhecido pelo sistema |
| ProductsAmount | Valor dos produtos |
| FreightAmount | Valor do frete |
| DiscountAmount | Valor do desconto |
| TotalAmount | Valor total do documento |
| IssueDate | Data de emissão |
| Number | Número do documento fiscal |
| Series | Série do documento fiscal |

As siglas fiscais brasileiras permanecem em sua nomenclatura oficial na linguagem do negócio e nas descrições apresentadas ao usuário.

Exemplos:

* NCM;
* CFOP;
* ICMS;
* IPI;
* PIS;
* COFINS;
* IBS;
* CBS.

No código C#, as siglas seguem a convenção de nomenclatura adotada pelo projeto, como `Ncm`, `Cfop`, `Nfe` e `Nfce`.

## Regras atuais do domínio

### FiscalDocument

* O número do documento fiscal deve ser maior que zero.
* A série do documento fiscal não pode ser negativa.
* Todo documento deve possuir uma chave de acesso.
* Valores de produtos, frete, desconto e total não podem ser negativos.
* O desconto deve respeitar os limites definidos pelas regras do documento.
* Os itens são adicionados através da raiz do agregado.
* Não podem existir itens com o mesmo número dentro do mesmo documento.
* Regras comuns entre NF-e e NFC-e devem permanecer em `FiscalDocument`.
* Regras específicas de um modelo não devem ser colocadas na classe base.

### AccessKey

* A chave de acesso é obrigatória.
* Deve possuir exatamente 44 dígitos.
* Deve conter somente caracteres numéricos.

### Ncm

* O NCM é obrigatório.
* Deve possuir exatamente 8 dígitos.
* Deve conter somente caracteres numéricos.

### Cfop

* O CFOP é obrigatório.
* Deve possuir exatamente 4 dígitos.
* Deve conter somente caracteres numéricos.

### Nfe

* A NF-e possui uma finalidade.
* Finalidades com comportamentos diferentes podem ser representadas por tipos específicos.
* Regras exclusivas de NF-e não devem ser adicionadas à `Nfce` ou à classe base `FiscalDocument`.

### ReturnNfe

* Uma NF-e de devolução deve ser identificada separadamente de uma operação normal.
* Valores fiscais presentes em uma devolução não devem ser somados automaticamente como novos valores de faturamento ou de tributos.
* A interpretação desses valores depende do tipo de análise realizada pelo sistema.

### CreditNfe

* Uma NF-e de crédito deve possuir uma finalidade compatível com crédito.
* Quando aplicável, deve possuir um `NfeCreditType` válido.

### DebitNfe

* Uma NF-e de débito deve possuir uma finalidade compatível com débito.
* Quando aplicável, deve possuir um `NfeDebitType` válido.

## Valores do documento e valores de análise

O FiscalZen diferencia o valor informado originalmente no documento fiscal do valor utilizado em determinada análise.

```text
Valor informado no XML
        ≠
Valor considerado no relatório
```

Uma NF-e de devolução pode possuir valores de tributos no XML, porém isso não significa que esses valores devam ser tratados da mesma forma que os tributos de uma operação normal.

O domínio preserva os dados originais do documento.

A interpretação desses valores para faturamento, apuração, indicadores ou relatórios ocorre através de regras específicas do domínio ou da aplicação.

Essa separação evita alterar o significado original do documento fiscal.

## Organização atual

A estrutura física do projeto é mantida de forma simples para evitar que a documentação precise ser alterada sempre que uma nova classe for criada.

```text
FiscalZen.Domain
│
├── Common
│   ├── Abstractions
│   └── Exceptions
│
└── FiscalDocuments
    ├── Entities
    ├── ValueObjects
    ├── Enums
    └── Services
```

A estrutura detalhada dos arquivos deve ser consultada diretamente no código-fonte.

A documentação deve priorizar conceitos, regras e decisões do domínio em vez de reproduzir toda a árvore de arquivos do projeto.

## Separação entre domínio e infraestrutura

O domínio não deve conhecer a forma como os documentos fiscais são obtidos.

Conceitos como:

* XML;
* arquivos;
* desserialização;
* banco de dados;
* Entity Framework;
* APIs externas;

não pertencem ao núcleo do domínio.

O XML é apenas uma fonte de dados utilizada para construir objetos do domínio.

```text
XML
 ↓
Parser
 ↓
Nfe / Nfce
 ↓
Domain
```

Os parsers permanecem fora do projeto `FiscalZen.Domain`.

## Evolução do domínio

A estrutura atual utiliza inicialmente um único projeto:

```text
FiscalZen.Domain
```

Dentro dele, os conceitos são agrupados por módulo de domínio.

Atualmente:

```text
FiscalDocuments
```

Caso novos conjuntos de regras e linguagens próprias apareçam, novos módulos poderão ser criados.

Exemplos possíveis:

```text
FiscalDocuments
Importing
Reporting
```

Esses módulos somente deverão ser considerados Bounded Contexts independentes quando houver diferenças suficientes de linguagem, regras e responsabilidade para justificar essa separação.

Caso isso aconteça, a estrutura poderá evoluir para algo semelhante a:

```text
FiscalZen.FiscalDocuments.Domain
FiscalZen.Importing.Domain
FiscalZen.Reporting.Domain
```

Essa separação física não deve ser realizada antecipadamente apenas por organização técnica.

## Diretrizes do domínio

* o código utiliza nomes em inglês;
* mensagens de erro do domínio utilizam PT-BR;
* siglas fiscais brasileiras mantêm sua nomenclatura oficial no domínio fiscal;
* as siglas seguem a convenção de nomenclatura do C# nos identificadores;
* regras devem permanecer próximas aos conceitos responsáveis por elas;
* Value Objects devem ser utilizados quando um valor possuir significado e regras próprias;
* herança deve representar diferenças reais de domínio;
* subclasses não devem ser criadas apenas para organizar código;
* estados inválidos devem ser evitados através do próprio modelo;
* Aggregates devem proteger suas invariantes;
* objetos internos de um agregado devem ser modificados através da Aggregate Root quando necessário;
* o Domain não deve depender de Infrastructure;
* o Domain não deve conhecer XML ou banco de dados;
* leitura do XML e interpretação fiscal devem permanecer separadas;
* a documentação deve representar o modelo e suas regras, não cada arquivo existente no projeto.
