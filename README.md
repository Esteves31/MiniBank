# MiniBank

API bancária desenvolvida em .NET 10 com Clean Architecture, servindo como base para um sistema de contas, transações e movimentações financeiras.

> **Status:** em desenvolvimento — Fase 1 (backend).

---

## Sobre o projeto

O MiniBank simula o núcleo de um sistema bancário: cadastro de clientes, abertura de contas e movimentações (depósito, saque) com registro de histórico de transações.

O objetivo do projeto vai além do CRUD: ele serve como estudo prático de arquitetura de software, aplicando separação de responsabilidades, inversão de dependência e domínio rico — princípios que sustentam sistemas de longa vida em ambientes corporativos.

O projeto é construído em fases incrementais, cada uma adicionando uma camada tecnológica ao sistema (veja o [Roadmap](#roadmap)).

---

## Arquitetura

O projeto segue **Clean Architecture**, organizado em quatro camadas com dependências direcionadas para o núcleo.

```
MiniBank/
├── src/
│   ├── MiniBank.Domain          # Entidades e regras de negócio
│   ├── MiniBank.Application     # Casos de uso, DTOs e contratos
│   ├── MiniBank.Infrastructure  # Persistência (EF Core)
│   └── MiniBank.Api             # Camada de apresentação (REST)
└── tests/
    └── MiniBank.Tests           # Testes unitários
```

### Responsabilidade de cada camada

| Camada | Responsabilidade | Depende de |
|---|---|---|
| **Domain** | Entidades (`Cliente`, `Conta`, `Transacao`), regras de negócio e exceções de domínio. Não conhece banco de dados nem framework web. | — |
| **Application** | Casos de uso (services), DTOs de entrada/saída e interfaces de repositório. Orquestra o domínio sem conter regras de negócio. | Domain |
| **Infrastructure** | Implementação de acesso a dados com Entity Framework Core: `DbContext`, mapeamentos e repositórios. | Application, Domain |
| **Api** | Controllers, middleware de tratamento de erros e configuração de injeção de dependência. | Application, Infrastructure |

### A regra de dependência

As dependências apontam sempre **para dentro**. O `Domain` não referencia nenhum outro projeto, o que garante que as regras de negócio permaneçam independentes de banco de dados, framework e detalhes de infraestrutura.

Essa restrição não é apenas convenção: ela é imposta pelas referências entre projetos, de modo que o próprio compilador impede violações da arquitetura.

### Domínio rico

As regras de negócio residem nas entidades, não nos services. A entidade `Conta` é responsável por proteger suas próprias invariantes — por exemplo, um saque não é validado pelo service, e sim recusado pela própria conta quando o saldo é insuficiente.

Como consequência, o núcleo do sistema é testável sem banco de dados, sem HTTP e sem mocks.

---

## Stack

| Categoria | Tecnologia |
|---|---|
| Linguagem | C# 12 |
| Framework | .NET 10 / ASP.NET Core |
| ORM | Entity Framework Core |
| Banco de dados | PostgreSQL |
| Documentação | Swagger / OpenAPI |
| Testes | xUnit + FluentAssertions |
| Containerização | Docker |

---

## Pré-requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/products/docker-desktop/) (para o banco de dados)
- Ferramenta de migrations do EF Core:

```bash
dotnet tool install --global dotnet-ef
```

---

## Como executar

**1. Clone o repositório**

```bash
git clone https://github.com/<seu-usuario>/MiniBank.git
cd MiniBank
```

**2. Suba o banco de dados**

```bash
docker compose up -d
```

**3. Aplique as migrations**

```bash
dotnet ef database update \
  --project src/MiniBank.Infrastructure \
  --startup-project src/MiniBank.Api
```

**4. Execute a API**

```bash
dotnet run --project src/MiniBank.Api
```

A documentação interativa estará disponível em `https://localhost:<porta>/swagger` (a porta é exibida no terminal).

**Executar os testes**

```bash
dotnet test
```

---

## Regras de negócio

**Cliente**
- Identificado por nome e CPF.
- O CPF é único no sistema.

**Conta**
- Pertence a um cliente e possui número único.
- É aberta com saldo zero.
- Toda movimentação gera um registro de transação no histórico.

**Depósito**
- O valor deve ser positivo.
- Incrementa o saldo e registra uma transação do tipo `Deposito`.

**Saque**
- O valor deve ser positivo.
- O valor não pode exceder o saldo disponível.
- Decrementa o saldo e registra uma transação do tipo `Saque`.

Violações de regra de negócio resultam em `DomainException`, traduzida pela camada de API em uma resposta `400 Bad Request`.

---

## Endpoints

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/contas` | Cria uma nova conta |
| `GET` | `/api/contas` | Lista as contas |
| `GET` | `/api/contas/{id}` | Detalha uma conta |
| `POST` | `/api/contas/{id}/depositar` | Realiza um depósito |
| `POST` | `/api/contas/{id}/sacar` | Realiza um saque |

**Exemplo — depósito**

```http
POST /api/contas/{id}/depositar
Content-Type: application/json

{
  "valor": 150.00
}
```

---

## Roadmap

- [ ] **Fase 1** — Backend REST em .NET 10 com Clean Architecture
- [ ] **Fase 2** — Frontend em Angular com arquitetura de Micro Frontends
- [ ] **Fase 3** — Mensageria assíncrona com Apache Kafka
- [ ] **Fase 4** — Deploy em AWS (ECS/Lambda, RDS, S3 + CloudFront) com IaC e CI/CD
- [ ] **Fase 5** — Cliente desktop administrativo em WPF (MVVM)
- [ ] **Fase 6** — Integração de IA aplicada ao ciclo de desenvolvimento

---

## Licença

Distribuído sob a licença MIT.