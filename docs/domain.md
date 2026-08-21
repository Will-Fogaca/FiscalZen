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
  NFe       NFCe
```

`FiscalDocument` concentra os dados e comportamentos comuns aos documentos fiscais.

`NFe` e `NFCe` representam as particularidades de cada modelo.

A NF-e possui ainda finalidades que podem alterar seu comportamento dentro do domínio.

```text
FiscalDocument
├── NFCe
└── NFe
    ├── NormalNFe
    ├── ReturnNFe
    ├── CreditNFe
    └── DebitNFe
```

Essas especializações existem porque determinadas finalidades da NF-e podem possuir regras e interpretações diferentes durante as análises fiscais.

## Principais conceitos

### FiscalDocument

Representa a base de um documento fiscal eletrônico.

Responsabilidades iniciais:

* possuir uma chave de acesso;
* possuir número e série;
* informar a data de emissão;
* armazenar valores monetários;
* controlar os itens do documento;
* garantir regras básicas de consistência.

As regras comuns entre NF-e e NFC-e devem permanecer nessa classe.

### NFe

Representa uma Nota Fiscal Eletrônica modelo 55.

Herda as características comuns de `FiscalDocument` e concentra comportamentos e informações específicos da NF-e.

A NF-e possui diferentes finalidades que podem alterar a forma como o documento deve ser interpretado pelo sistema.

### NormalNFe

Representa uma NF-e de finalidade normal.

É utilizada para operações comuns realizadas através da NF-e, respeitando as regras específicas da operação e da tributação informada no documento.

### ReturnNFe

Representa uma NF-e de devolução.

Uma devolução possui significado diferente de uma operação normal e seus valores não devem ser tratados automaticamente como novos valores de faturamento ou tributação.

A interpretação dos valores de uma devolução deve respeitar as regras fiscais aplicáveis ao documento e aos relatórios gerados pelo sistema.

### CreditNFe

Representa uma NF-e de crédito.

Além da finalidade de crédito, esse documento pode possuir um tipo específico representado pelo enum `NFeCreditType`.

O tipo de crédito poderá influenciar regras fiscais e análises realizadas pelo sistema.

### DebitNFe

Representa uma NF-e de débito.

Além da finalidade de débito, esse documento pode possuir um tipo específico representado pelo enum `NFeDebitType`.

O tipo de débito poderá influenciar regras fiscais e análises realizadas pelo sistema.

### NFCe

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
* CST ou CSOSN;
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

O próprio Value Object pode fornecer comportamentos relacionados a valores monetários, como soma, subtração e comparação.

## Enums

### NFePurpose

Representa a finalidade de uma NF-e.

As finalidades fazem parte do significado do documento e podem alterar a forma como seus valores são interpretados pelo sistema.

Exemplos:

* Normal;
* Complementary;
* Adjustment;
* Return;
* Credit;
* Debit.

Sempre que uma finalidade possuir comportamento próprio relevante para o domínio, poderá existir uma especialização de `NFe` correspondente.

### NFeCreditType

Representa o tipo específico de uma NF-e com finalidade de crédito.

Esse enum utiliza os códigos definidos para o documento fiscal e mantém os nomes do código em inglês.

Descrições em PT-BR podem ser associadas aos valores para utilização em relatórios e interfaces.

### NFeDebitType

Representa o tipo específico de uma NF-e com finalidade de débito.

Assim como `NFeCreditType`, os valores representam códigos fiscais oficiais e podem possuir descrições em PT-BR para apresentação ao usuário.

## Linguagem ubíqua

| Termo no código    | Significado no domínio                         |
| ------------------ | ---------------------------------------------- |
| FiscalDocument     | Documento fiscal eletrônico                    |
| NFe                | Nota Fiscal Eletrônica modelo 55               |
| NFCe               | Nota Fiscal de Consumidor Eletrônica modelo 65 |
| NormalNFe          | NF-e de finalidade normal                      |
| ReturnNFe          | NF-e de devolução                              |
| CreditNFe          | NF-e de crédito                                |
| DebitNFe           | NF-e de débito                                 |
| NFePurpose         | Finalidade da NF-e                             |
| NFeCreditType      | Tipo específico de NF-e de crédito             |
| NFeDebitType       | Tipo específico de NF-e de débito              |
| FiscalDocumentItem | Item do documento fiscal                       |
| AccessKey          | Chave de acesso do documento fiscal            |
| Money              | Valor monetário                                |
| ProductsAmount     | Valor dos produtos                             |
| FreightAmount      | Valor do frete                                 |
| DiscountAmount     | Valor do desconto                              |
| TotalAmount        | Valor total do documento                       |
| IssueDate          | Data de emissão                                |
| Number             | Número do documento fiscal                     |
| Series             | Série do documento fiscal                      |

As siglas fiscais brasileiras permanecem em sua nomenclatura oficial.

Exemplos:

* NCM;
* CFOP;
* CST;
* CSOSN;
* ICMS;
* IPI;
* PIS;
* COFINS;
* IBS;
* CBS.

## Regras iniciais do domínio

### FiscalDocument

* O número do documento fiscal deve ser maior que zero.
* A série do documento fiscal não pode ser negativa.
* Todo documento deve possuir uma chave de acesso.
* Valores de produtos, frete e desconto não podem ser negativos.
* O desconto deve respeitar os limites definidos pelas regras do documento.
* Regras comuns entre NF-e e NFC-e devem permanecer em `FiscalDocument`.
* Regras específicas de um modelo não devem ser colocadas na classe base.

### AccessKey

* A chave de acesso é obrigatória.
* Deve possuir exatamente 44 dígitos.
* Deve conter somente caracteres numéricos.

### NFe

* A NF-e possui uma finalidade.
* Finalidades com comportamentos diferentes podem ser representadas por tipos específicos.
* Regras exclusivas de NF-e não devem ser adicionadas à `NFCe` ou à classe base `FiscalDocument`.

### ReturnNFe

* Uma NF-e de devolução deve ser identificada separadamente de uma operação normal.
* Valores fiscais presentes em uma devolução não devem ser somados automaticamente como novos valores de faturamento ou de tributos.
* A interpretação desses valores depende do tipo de análise realizada pelo sistema.

### CreditNFe

* Uma NF-e de crédito deve possuir uma finalidade compatível com crédito.
* Quando aplicável, deve possuir um `NFeCreditType` válido.

### DebitNFe

* Uma NF-e de débito deve possuir uma finalidade compatível com débito.
* Quando aplicável, deve possuir um `NFeDebitType` válido.

## Valores do documento e valores de análise

O FiscalZen deve diferenciar o valor informado originalmente no documento fiscal do valor utilizado em determinada análise.

Por exemplo:

```text
Valor informado no XML
        ≠
Valor considerado no relatório
```

Uma NF-e de devolução pode possuir valores de tributos no XML, porém isso não significa que esses valores devam ser tratados da mesma forma que os tributos de uma operação normal.

O domínio deve preservar os dados originais do documento.

A interpretação desses valores para faturamento, apuração, indicadores ou relatórios deve ocorrer através de regras específicas do domínio ou da aplicação.

Essa separação evita alterar o significado original do documento fiscal.

## Organização atual

A estrutura física do projeto é mantida de forma simples para evitar que a documentação precise ser alterada sempre que uma nova classe for criada.

```text
FiscalZen.Domain
│
├── FiscalDocuments
│   ├── Entities
│   ├── ValueObjects
│   └── Enums
│
└── Common
    └── Exceptions
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

Exemplo:

```text
XML
 ↓
Parser
 ↓
NFe / NFCe
 ↓
Domain
```

Os parsers devem permanecer fora do projeto `FiscalZen.Domain`.

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

O projeto seguirá inicialmente as seguintes diretrizes:

* o código utiliza nomes em inglês;
* mensagens de erro do domínio utilizam PT-BR;
* siglas fiscais brasileiras mantêm sua nomenclatura oficial;
* regras devem permanecer próximas aos conceitos responsáveis por elas;
* Value Objects devem ser utilizados quando um valor possuir significado e regras próprias;
* herança deve representar diferenças reais de domínio;
* subclasses não devem ser criadas apenas para organizar código;
* estados inválidos devem ser evitados através do próprio modelo;
* o Domain não deve depender de Infrastructure;
* o Domain não deve conhecer XML ou banco de dados;
* a documentação deve representar o modelo e suas regras, não cada arquivo existente no projeto.
