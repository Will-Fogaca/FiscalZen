# Architecture

## Visão geral

O FiscalZen utiliza uma arquitetura baseada em princípios de **Clean Architecture** e **Domain-Driven Design**.

O objetivo principal é manter as regras de domínio independentes de tecnologias externas como:

* banco de dados;
* leitura de XML;
* APIs;
* frameworks;
* interface gráfica;
* dashboards.

## Estrutura da solução

```text
FiscalZen
│
├── FiscalZen.Domain
├── FiscalZen.Application
├── FiscalZen.Infrastructure
└── FiscalZen.Api
```

## Domain

O projeto:

```text
FiscalZen.Domain
```

contém os conceitos e regras de negócio.

O Domain não deve depender de:

* Entity Framework;
* banco de dados;
* XML;
* APIs externas;
* interface do usuário.

Exemplos de objetos pertencentes ao Domain:

```text
FiscalDocument
NFe
NFCe
FiscalDocumentItem
AccessKey
Money
```

## Application

O projeto:

```text
FiscalZen.Application
```

é responsável pelos casos de uso da aplicação.

Exemplos:

```text
ImportFiscalDocuments
GetRevenueReport
GetTaxesReport
GetFreightReport
```

A Application coordena o fluxo da aplicação, mas não deve concentrar regras pertencentes ao domínio.

Exemplo:

```text
Usuário solicita importação
        ↓
Application recebe comando
        ↓
Obtém os dados do XML
        ↓
Cria objetos do Domain
        ↓
Solicita persistência
```

## Infrastructure

O projeto:

```text
FiscalZen.Infrastructure
```

contém implementações relacionadas a tecnologias externas.

Exemplos:

* leitura de arquivos XML;
* desserialização de NF-e;
* desserialização de NFC-e;
* banco de dados;
* Entity Framework;
* implementação dos repositórios.

Estrutura esperada:

```text
FiscalZen.Infrastructure
│
├── Persistence
│
└── Xml
    ├── NFeXmlParser.cs
    └── NFCeXmlParser.cs
```

O Domain não deve conhecer `NFeXmlParser` ou `NFCeXmlParser`.

O XML é apenas uma forma de entrada de dados.

## API

O projeto:

```text
FiscalZen.Api
```

é responsável por disponibilizar as funcionalidades da aplicação para o cliente.

Suas responsabilidades podem incluir:

* endpoints;
* autenticação;
* validação básica da requisição;
* upload dos XMLs;
* chamadas para a camada Application.

A API não deve conter regras de negócio.

## Dependências

As dependências devem seguir o sentido das camadas internas.

```text
              ┌──────────────┐
              │     API      │
              └──────┬───────┘
                     │
                     ▼
             ┌───────────────┐
             │  Application  │
             └───────┬───────┘
                     │
                     ▼
             ┌───────────────┐
             │    Domain     │
             └───────────────┘
```

A Infrastructure implementa recursos necessários pelas demais camadas:

```text
Infrastructure
      │
      ├── Persistence
      ├── XML
      └── integrações externas
```

## Fluxo de importação

O fluxo inicial esperado é:

```text
Usuário
   ↓
Seleciona arquivos XML
   ↓
Seleciona NF-e ou NFC-e
   ↓
API
   ↓
Application
   ↓
XML Parser
   ↓
FiscalDocument
   ↓
NFe / NFCe
   ↓
Repository
   ↓
Database
```

## Fluxo de relatórios

Os dashboards possuem características principalmente de leitura.

Por esse motivo, consultas de relatório não precisam obrigatoriamente carregar Aggregates completos do domínio.

Um fluxo possível é:

```text
Database
   ↓
Reporting Query
   ↓
DTO
   ↓
API
   ↓
Dashboard
```

Isso evita carregar milhares de documentos e itens apenas para executar operações como:

```text
SUM
COUNT
AVG
GROUP BY
```

## Organização por domínio

Inicialmente será utilizado um único projeto:

```text
FiscalZen.Domain
```

Dentro dele, os conceitos serão separados por módulos:

```text
FiscalZen.Domain
├── FiscalDocuments
└── Common
```

Caso o sistema cresça e novos Bounded Contexts sejam identificados, esses módulos poderão ser separados posteriormente.

Exemplo:

```text
FiscalZen.FiscalDocuments.Domain
FiscalZen.Reporting.Domain
FiscalZen.Importing.Domain
```

A separação física ocorrerá somente quando houver necessidade real.

## Princípios adotados

O projeto seguirá inicialmente os seguintes princípios:

* Domain não conhece Infrastructure.
* Domain não conhece XML.
* Domain não conhece banco de dados.
* As regras do domínio ficam próximas dos objetos responsáveis por elas.
* Value Objects são utilizados para conceitos com significado e regras próprias.
* O código utiliza nomes em inglês.
* Mensagens apresentadas pelo domínio utilizam PT-BR.
* Siglas fiscais brasileiras mantêm seus nomes oficiais.
* Novos Bounded Contexts são criados apenas quando houver necessidade de domínio.
