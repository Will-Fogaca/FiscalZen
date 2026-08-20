# Domain Model

## Visão geral

O domínio do FiscalZen é responsável por representar os conceitos e regras relacionados aos documentos fiscais utilizados pela aplicação.

Neste primeiro momento, o sistema possui um módulo principal:

```text
FiscalDocuments
```

Esse módulo concentra os conceitos relacionados às NF-e e NFC-e analisadas pelo sistema.

## Fiscal Documents

O módulo `FiscalDocuments` é responsável pela representação dos documentos fiscais eletrônicos.

Inicialmente são suportados:

* NF-e — modelo 55;
* NFC-e — modelo 65.

Ambos compartilham características comuns e são representados através da classe base:

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

### NFe

Representa uma Nota Fiscal Eletrônica modelo 55.

Herda as características comuns de `FiscalDocument` e poderá possuir regras e informações específicas de NF-e.

### NFCe

Representa uma Nota Fiscal de Consumidor Eletrônica modelo 65.

Herda as características comuns de `FiscalDocument` e poderá possuir regras e informações específicas de NFC-e.

### FiscalDocumentItem

Representa um item existente dentro de um documento fiscal.

Poderá conter informações como:

* código do produto;
* descrição;
* NCM;
* CFOP;
* CST ou CSOSN;
* quantidade;
* valor unitário;
* valor total;
* tributos relacionados ao item.

### AccessKey

`AccessKey` é um Value Object que representa a chave de acesso de um documento fiscal.

Regras iniciais:

* deve ser informada;
* deve possuir 44 dígitos;
* deve conter apenas números;
* é tratada como um valor imutável.

### Money

`Money` é um Value Object utilizado para representar valores monetários dentro do domínio.

Exemplos:

* valor dos produtos;
* frete;
* descontos;
* valor total;
* valores tributários.

O uso de `Money` evita representar conceitos monetários diretamente através de `decimal` em todo o domínio.

## Linguagem ubíqua

| Termo no código    | Significado no domínio                         |
| ------------------ | ---------------------------------------------- |
| FiscalDocument     | Documento fiscal eletrônico                    |
| NFe                | Nota Fiscal Eletrônica modelo 55               |
| NFCe               | Nota Fiscal de Consumidor Eletrônica modelo 65 |
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
* A série não pode ser negativa.
* Todo documento deve possuir uma chave de acesso.
* Valores monetários utilizados como produtos, frete e desconto não podem ser negativos.
* O desconto não pode ultrapassar o valor permitido pelas regras do documento.

### AccessKey

* A chave de acesso é obrigatória.
* Deve possuir exatamente 44 dígitos.
* Deve conter somente caracteres numéricos.

## Organização atual

```text
FiscalZen.Domain
│
├── FiscalDocuments
│   │
│   ├── Entities
│   │   ├── FiscalDocument.cs
│   │   ├── NFe.cs
│   │   ├── NFCe.cs
│   │   └── FiscalDocumentItem.cs
│   │
│   ├── ValueObjects
│   │   ├── AccessKey.cs
│   │   └── Money.cs
│
└── Common
    └── Exceptions
        └── DomainException.cs
```

## Evolução do domínio

A estrutura atual não impede a criação futura de novos Bounded Contexts.

Caso novos domínios com regras próprias apareçam, módulos poderão ser extraídos para contextos independentes.

Exemplos futuros:

```text
FiscalDocuments
Reporting
Importing
```

Essa separação somente deverá acontecer quando houver complexidade de domínio suficiente para justificá-la.
