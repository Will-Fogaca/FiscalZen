# FiscalZen

## Sobre o projeto

O **FiscalZen** é uma aplicação para importação, armazenamento, apuração e análise de documentos fiscais eletrônicos.

O sistema foi desenvolvido com foco na leitura de arquivos XML e na transformação desses dados em objetos de domínio, permitindo posteriormente realizar:

* persistência;
* apuração tributária;
* consultas;
* relatórios;
* dashboards.

Atualmente, o desenvolvimento está concentrado principalmente em:

* NF-e — Nota Fiscal Eletrônica, modelo 55.

O suporte para:

* NFC-e — Nota Fiscal de Consumidor Eletrônica, modelo 65;

faz parte da evolução do projeto.

## Objetivo

O objetivo do FiscalZen é facilitar a análise de informações fiscais e financeiras presentes nos documentos fiscais eletrônicos.

Entre as informações que poderão ser analisadas estão:

* faturamento;
* valor dos produtos;
* frete;
* descontos;
* ICMS;
* IPI;
* PIS;
* COFINS;
* IBS;
* CBS;
* informações dos produtos;
* NCM;
* CFOP;
* regime tributário;
* finalidade da NF-e;
* informações dos itens dos documentos.

## Arquitetura

O projeto utiliza conceitos de:

* Clean Architecture;
* Domain-Driven Design — DDD;
* SOLID;
* Value Objects;
* Aggregates;
* Aggregate Roots;
* Domain Services;
* Strategy Pattern.

Estrutura da solução:

```text
FiscalZen
│
├── src
│   ├── FiscalZen.Domain
│   ├── FiscalZen.Application
│   ├── FiscalZen.Infrastructure
│   └── FiscalZen.Api
│
└── tests
    ├── FiscalZen.Domain.Tests
    └── FiscalZen.Infrastructure.Tests