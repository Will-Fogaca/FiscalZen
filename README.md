# FiscalZen

## Sobre o projeto

O **FiscalZen** é uma aplicação para importação e análise de documentos fiscais eletrônicos.

Inicialmente, o sistema terá suporte para:

* NF-e — Nota Fiscal Eletrônica, modelo 55;
* NFC-e — Nota Fiscal de Consumidor Eletrônica, modelo 65.

O usuário poderá selecionar uma lista de arquivos XML, informar o modelo do documento fiscal e realizar a importação.

Os dados extraídos serão armazenados em uma fonte de dados e utilizados para geração de relatórios e dashboards.

## Objetivo

O objetivo do FiscalZen é facilitar a análise de informações fiscais e financeiras presentes nos XMLs dos documentos fiscais.

Entre as informações que poderão ser analisadas estão:

* faturamento;
* valor dos produtos;
* frete;
* descontos;
* IPI;
* ICMS;
* PIS;
* COFINS;
* IBS;
* CBS;
* informações dos produtos;
* informações do emitente e destinatário.

## Arquitetura

O projeto utiliza conceitos de:

* Clean Architecture;
* Domain-Driven Design (DDD);
* separação entre domínio, aplicação e infraestrutura.

Estrutura inicial:

```text
src/
├── FiscalZen.Domain
├── FiscalZen.Application
├── FiscalZen.Infrastructure
└── FiscalZen.Api
```

## Domínio

Inicialmente, o principal módulo do domínio é:

```text
FiscalDocuments
```

Responsável por representar documentos fiscais e suas regras.

Estrutura inicial:

```text
FiscalZen.Domain
│
├── FiscalDocuments
│   ├── Entities
│   ├── ValueObjects
│   ├── Enums
│   └── Services
│
└── Common
    └── Exceptions
```

Os principais conceitos do domínio atualmente são:

* `FiscalDocument`;
* `NFe`;
* `NFCe`;
* `FiscalDocumentItem`;
* `AccessKey`;
* `Money`.

## Fluxo inicial

```text
XMLs
  ↓
Importação
  ↓
Leitura e interpretação
  ↓
NFe / NFCe
  ↓
Persistência
  ↓
Relatórios
  ↓
Dashboards
```

## Status

O projeto encontra-se em desenvolvimento inicial.
