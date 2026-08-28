# Architecture

## Visão geral

O FiscalZen utiliza uma arquitetura baseada em princípios de **Clean Architecture**, **Domain-Driven Design** e **SOLID**.

O objetivo é manter as regras de negócio independentes de tecnologias externas como:

* banco de dados;
* XML;
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

contém os conceitos, comportamentos e regras do domínio.

O Domain não deve depender de:

* Infrastructure;
* API;
* Entity Framework;
* banco de dados;
* XML;
* frameworks externos de persistência.

Principais conceitos:

```text
FiscalDocument
Nfe
Nfce
FiscalDocumentItem
AccessKey
Money
Ncm
Cfop
TaxSummary
TaxRegime
```

### Aggregate

`FiscalDocument` representa a raiz do agregado de documentos fiscais.

```text
FiscalDocument ← Aggregate Root
│
└── FiscalDocumentItem
```

A raiz controla as alterações realizadas no agregado.

Exemplo:

```text
FiscalDocument.AddItem(...)
```

em vez de permitir alteração direta da coleção de itens.

A existência de um Aggregate não define a estrutura física do banco de dados.

Um agregado pode ser persistido através de múltiplas tabelas.

Exemplo:

```text
FiscalDocuments
       1
       │
       N
FiscalDocumentItems
```

Mesmo existindo duas tabelas, o acesso de domínio ocorre através de `FiscalDocument`.

## Application

O projeto:

```text
FiscalZen.Application
```

contém os casos de uso e contratos necessários para execução da aplicação.

Exemplos atuais:

```text
ImportFiscalDocumentsUseCase
IXmlFiscalDocumentParser
```

A Application coordena os fluxos, mas não deve implementar regras pertencentes ao domínio.

Exemplo:

```text
XML
 ↓
Application
 ↓
IXmlFiscalDocumentParser
 ↓
FiscalDocument
```

## Infrastructure

O projeto:

```text
FiscalZen.Infrastructure
```

contém implementações relacionadas a tecnologias externas.

Atualmente:

```text
FiscalZen.Infrastructure
└── Xml
    └── NFeXmlParser.cs
```

O `NFeXmlParser` implementa o contrato definido pela Application:

```text
IXmlFiscalDocumentParser
        ▲
        │
NFeXmlParser
```

A Infrastructure também será responsável por:

* banco de dados;
* Entity Framework;
* repositories;
* persistência;
* integrações externas.

## API

O projeto:

```text
FiscalZen.Api
```

é o ponto de entrada da aplicação.

Suas responsabilidades incluem:

* endpoints;
* upload de arquivos;
* autenticação;
* validações de entrada;
* execução dos casos de uso;
* configuração de Dependency Injection.

A API funciona também como **Composition Root**, conectando abstrações às implementações.

Exemplo:

```text
IXmlFiscalDocumentParser
        ↓
NFeXmlParser
```

## Dependências

As dependências seguem a direção das camadas internas.

```text
Domain
  ↑
  │
Application
  ↑
  │
Infrastructure

Api
├── Application
├── Infrastructure
└── Domain
```

De forma simplificada:

```text
Domain
↑
Application
↑
Infrastructure
↑
Api
```

A API pode referenciar Infrastructure para registrar implementações concretas através de Dependency Injection.

O sentido inverso não é permitido.

Exemplos inválidos:

```text
Domain → Infrastructure
Domain → Api
Application → Api
Infrastructure → Api
```

## Fluxo de importação

```text
Usuário
   ↓
API
   ↓
ImportFiscalDocumentsUseCase
   ↓
IXmlFiscalDocumentParser
   ↓
NFeXmlParser
   ↓
FiscalDocument
   ↓
Domain
```

O parser é responsável somente por interpretar a estrutura do XML e construir objetos válidos do domínio.

## Parser

O XML pertence à Infrastructure.

```text
XML
 ↓
NFeXmlParser
 ↓
FiscalDocument
```

Atualmente o parser extrai:

* identificação;
* finalidade;
* regime tributário disponível no XML;
* valores totais;
* itens;
* NCM;
* CFOP;
* tributos do documento;
* tributos dos itens.

O Domain não possui dependência de `System.Xml.Linq`.

## Persistência

Repositories devem trabalhar principalmente com **Aggregate Roots**.

Exemplo:

```text
IFiscalDocumentRepository
        ↓
FiscalDocument
```

Não é necessário criar repositories independentes para objetos internos do agregado.

Exemplo:

```text
FiscalDocumentItemRepository
```

não será necessário enquanto `FiscalDocumentItem` permanecer pertencente exclusivamente ao agregado `FiscalDocument`.

No banco, entretanto, itens podem possuir sua própria tabela.

## Apuração tributária

A interpretação fiscal não é responsabilidade do parser.

```text
XML
 ↓
TaxSummary
 ↓
Domain Rules
 ↓
Tax Assessment
```

O parser preserva os valores encontrados no XML.

As regras de apuração determinam como esses valores impactam determinada análise.

Isso mantém separadas as responsabilidades de:

```text
Leitura
≠
Regra fiscal
```

## Relatórios

Relatórios e dashboards possuem características principalmente de leitura.

Por isso, consultas analíticas não precisam obrigatoriamente reconstruir Aggregates completos.

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

Isso permite operações otimizadas como:

```text
SUM
COUNT
AVG
GROUP BY
```

sem carregar milhares de objetos do domínio.

## Organização por domínio

Inicialmente existe um único projeto:

```text
FiscalZen.Domain
```

organizado por módulos.

```text
FiscalZen.Domain
├── Common
└── FiscalDocuments
```

Novos Bounded Contexts somente devem ser extraídos quando existirem diferenças reais de linguagem, regras ou responsabilidade.

## Princípios adotados

* Domain não conhece Infrastructure.
* Domain não conhece XML.
* Domain não conhece banco de dados.
* Application coordena casos de uso.
* Infrastructure implementa detalhes técnicos.
* API atua como ponto de entrada e Composition Root.
* Repositories trabalham com Aggregate Roots.
* Value Objects representam conceitos com regras próprias.
* Estados inválidos devem ser evitados pelo modelo.
* Dados originais do XML devem ser preservados.
* Leitura de XML e interpretação fiscal são responsabilidades distintas.
* Novos Bounded Contexts não devem ser criados antecipadamente.
