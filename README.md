# Mechanical Workshop API

Sistema Integrado de Gestão de Serviços para uma oficina mecânica para o Tech Challenge Fase 1 da Pós Tech em Arquitetura de Software da FIAP.

🇺🇸 [English version](#mechanical-workshop-api-1)

## Visão Geral

Este sistema substitui processos manuais por uma plataforma digital que permite gestão de ordens de serviço, acompanhamento de clientes, controle de estoque de peças e monitoramento de status em tempo real. A arquitetura segue princípios de Domain-Driven Design (DDD) organizada como monolito em camadas.

## Stack Tecnológica

| Componente | Tecnologia |
|------------|-----------|
| Runtime | .NET 10 |
| Linguagem | C# |
| Banco de Dados | PostgreSQL 16 |
| ORM | Entity Framework Core 10 |
| Autenticação | JWT Bearer Token |
| Documentação | Swagger (Swashbuckle) |
| Validação | FluentValidation |
| Containerização | Docker + Docker Compose |
| Arquitetura | DDD — Monolito em Camadas |

## Estrutura do Projeto

```
MechanicalWorkshop/
├── MechanicalWorkshop.Domain/          # Entidades, Value Objects, Enums, Interfaces
│   ├── Entities/                       # Customer, Vehicle, ServiceOrder, Part, Service, AdminUser
│   ├── ValueObjects/                   # Cpf, Cnpj, LicensePlate (com validação)
│   ├── Enums/                          # ServiceOrderStatus
│   ├── Exceptions/                     # DomainException, InvalidStatusTransitionException
│   └── Interfaces/Repositories/        # Contratos de repositório + IUnitOfWork
│
├── MechanicalWorkshop.Application/     # Casos de Uso, DTOs, Validadores
│   ├── UseCases/                       # Organizados por agregado (Customers, Vehicles, etc.)
│   ├── DTOs/                           # Records de Request/Response
│   ├── Validators/                     # Regras FluentValidation
│   └── Interfaces/                     # IPasswordHasher, IJwtTokenGenerator
│
├── MechanicalWorkshop.Infrastructure/  # EF Core, Repositórios, Segurança
│   ├── Persistence/
│   │   ├── Context/                    # AppDbContext + DesignTimeFactory
│   │   ├── Mappings/                   # Configurações Fluent API
│   │   ├── Repositories/              # Implementação dos repositórios
│   │   └── Migrations/                # Migrations do EF Core
│   └── Security/                       # PasswordHasher, JwtTokenGenerator
│
├── MechanicalWorkshop.API/             # Controllers, Middlewares, Extensions
│   ├── Controllers/                    # Auth, Customers, Vehicles, Services, Parts, ServiceOrders
│   ├── Middlewares/                    # ExceptionMiddleware, ValidationFilter
│   └── Extensions/                    # DI, JWT, Swagger, Database config
│
├── Dockerfile
├── docker-compose.yml
└── README.md
```

## Grafo de Dependências

```
API → Application → Domain
API → Infrastructure → Application → Domain
```

A camada Domain não possui dependências externas. Infrastructure implementa os contratos definidos pelo Domain. Application orquestra a lógica de domínio. API é o ponto de entrada.

## Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e em execução
- (Opcional) [.NET 10 SDK](https://dotnet.microsoft.com/download) para desenvolvimento local

## Como Executar

### Com Docker (recomendado)

```bash
git clone <url-do-repositorio>
cd tech-challenge

docker compose up --build
```

A API estará disponível em `http://localhost:8080`.

Swagger UI: `http://localhost:8080/swagger`

### Localmente

```bash
# Certifique-se de que o PostgreSQL está rodando em localhost:5432
# Atualize a connection string no appsettings.json se necessário

dotnet restore
dotnet build
dotnet run --project MechanicalWorkshop.API
```

## Autenticação

O sistema utiliza tokens JWT Bearer para endpoints administrativos. Um usuário admin padrão é criado na primeira execução:

| Campo | Valor |
|-------|-------|
| Email | `admin@workshop.com` |
| Senha | `Admin@123` |

### Login

```bash
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@workshop.com",
  "password": "Admin@123"
}
```

A resposta inclui um token JWT. Utilize-o no header `Authorization`:

```
Authorization: Bearer <token>
```

No Swagger UI, clique no botão "Authorize" e cole o token.

## Endpoints da API

### Autenticação
| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| POST | `/api/auth/login` | Não | Autenticar e obter token JWT |

### Clientes
| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| POST | `/api/customers` | Sim | Criar cliente |
| GET | `/api/customers` | Sim | Listar todos (filtro opcional `?name=`) |
| GET | `/api/customers/{id}` | Sim | Buscar por ID |
| GET | `/api/customers/document/{doc}` | Sim | Buscar por CPF/CNPJ |
| PUT | `/api/customers/{id}` | Sim | Atualizar cliente |
| DELETE | `/api/customers/{id}` | Sim | Excluir cliente |

### Veículos
| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| POST | `/api/vehicles` | Sim | Criar veículo |
| GET | `/api/vehicles` | Sim | Listar todos |
| GET | `/api/vehicles/{id}` | Sim | Buscar por ID |
| GET | `/api/vehicles/plate/{plate}` | Sim | Buscar por placa |
| GET | `/api/vehicles/customer/{id}` | Sim | Buscar por cliente |
| PUT | `/api/vehicles/{id}` | Sim | Atualizar veículo |
| DELETE | `/api/vehicles/{id}` | Sim | Excluir veículo |

### Serviços
| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| POST | `/api/services` | Sim | Criar serviço |
| GET | `/api/services` | Sim | Listar todos |
| GET | `/api/services/{id}` | Sim | Buscar por ID |
| GET | `/api/services/active` | Sim | Listar apenas ativos |
| PUT | `/api/services/{id}` | Sim | Atualizar serviço |
| PATCH | `/api/services/{id}/activate` | Sim | Ativar |
| PATCH | `/api/services/{id}/deactivate` | Sim | Desativar |
| DELETE | `/api/services/{id}` | Sim | Excluir serviço |

### Peças e Estoque
| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| POST | `/api/parts` | Sim | Criar peça |
| GET | `/api/parts` | Sim | Listar todas |
| GET | `/api/parts/{id}` | Sim | Buscar por ID |
| GET | `/api/parts/active` | Sim | Listar apenas ativas |
| GET | `/api/parts/low-stock` | Sim | Listar com estoque baixo |
| PUT | `/api/parts/{id}` | Sim | Atualizar informações |
| PATCH | `/api/parts/{id}/add-stock` | Sim | Adicionar estoque |
| PATCH | `/api/parts/{id}/remove-stock` | Sim | Remover estoque |
| PATCH | `/api/parts/{id}/activate` | Sim | Ativar |
| PATCH | `/api/parts/{id}/deactivate` | Sim | Desativar |
| DELETE | `/api/parts/{id}` | Sim | Excluir peça |

### Ordens de Serviço
| Método | Endpoint | Auth | Descrição |
|--------|----------|------|-----------|
| POST | `/api/service-orders` | Sim | Criar OS com itens |
| GET | `/api/service-orders` | Sim | Listar todas |
| GET | `/api/service-orders/{id}` | Sim | Detalhes completos |
| GET | `/api/service-orders/number/{num}` | **Não** | Público: consultar por número da OS |
| GET | `/api/service-orders/customer/{id}` | Sim | Buscar por cliente |
| GET | `/api/service-orders/status/{status}` | Sim | Filtrar por status |
| GET | `/api/service-orders/average-execution-time` | Sim | Tempo médio de execução |
| PATCH | `/api/service-orders/{id}/status` | Sim | Atualizar status |
| POST | `/api/service-orders/{id}/items` | Sim | Adicionar itens à OS |
| DELETE | `/api/service-orders/{id}` | Sim | Excluir OS |

## Fluxo de Status da Ordem de Serviço

```
Recebida → Em Diagnóstico → Aguardando Aprovação → Em Execução → Finalizada → Entregue
```

Cada transição é validada pelo domínio — transições inválidas (ex: `Recebida → Finalizada`) são rejeitadas automaticamente.

## Validações

Todas as entradas são validadas com FluentValidation:

- **CPF**: Validação completa com dígitos verificadores
- **CNPJ**: Validação completa com dígitos verificadores
- **Placa**: Suporta formato antigo (`ABC1234`) e Mercosul (`ABC1D23`)
- **Campos obrigatórios**: Nome, email, telefone, preços, quantidades
- **Regras de negócio**: Documentos únicos, placas únicas, disponibilidade de estoque

## Banco de Dados

O PostgreSQL 16 foi escolhido por sua robustez, licença open source, suporte nativo a UUID, operador `ILike` para busca case-insensitive e excelente compatibilidade com o EF Core.

Os dados são persistidos via volume Docker (`postgres_data`). Na primeira execução, o sistema aplica as migrations automaticamente e realiza o seed de dados de exemplo (usuário admin, 8 serviços, 8 peças).

## Variáveis de Ambiente

| Variável | Padrão | Descrição |
|----------|--------|-----------|
| `ConnectionStrings__DefaultConnection` | Ver docker-compose | Conexão PostgreSQL |
| `Jwt__Secret` | (configurado) | Chave de assinatura JWT |
| `Jwt__Issuer` | MechanicalWorkshop.API | Emissor do token |
| `Jwt__Audience` | MechanicalWorkshop.Client | Audiência do token |
| `Jwt__ExpirationHours` | 8 | Validade do token |

## Documentação DDD

A documentação completa de Domain-Driven Design incluindo Event Storming, agregados e linguagem ubíqua está disponível em:

**[Miro Board — Event Storming](https://miro.com/app/board/uXjVHdb4J3E=/?share_link_id=527909954955)**

## Licença

Este projeto faz parte do programa Pós Tech em Arquitetura de Software da FIAP — Tech Challenge Fase 1.

# Mechanical Workshop API

🇧🇷 [Versão em Português](#mechanical-workshop-api)

Integrated Service Management System for the FIAP Pós Tech Software Architecture Phase 1 Tech Challenge.

## Overview

This system replaces manual processes with a digital platform that enables service order management, customer tracking, parts inventory control, and real-time status monitoring. The architecture follows Domain-Driven Design (DDD) principles organized as a layered monolith.

## Tech Stack

| Component | Technology |
|-----------|-----------|
| Runtime | .NET 10 |
| Language | C# |
| Database | PostgreSQL 16 |
| ORM | Entity Framework Core 10 |
| Auth | JWT Bearer Token |
| Docs | Swagger (Swashbuckle) |
| Validation | FluentValidation |
| Containerization | Docker + Docker Compose |
| Architecture | DDD — Layered Monolith |

## Project Structure

```
MechanicalWorkshop/
├── MechanicalWorkshop.Domain/          # Entities, Value Objects, Enums, Interfaces
│   ├── Entities/                       # Customer, Vehicle, ServiceOrder, Part, Service, AdminUser
│   ├── ValueObjects/                   # Cpf, Cnpj, LicensePlate (with validation)
│   ├── Enums/                          # ServiceOrderStatus
│   ├── Exceptions/                     # DomainException, InvalidStatusTransitionException
│   └── Interfaces/Repositories/        # Repository contracts + IUnitOfWork
│
├── MechanicalWorkshop.Application/     # Use Cases, DTOs, Validators
│   ├── UseCases/                       # Organized by aggregate (Customers, Vehicles, etc.)
│   ├── DTOs/                           # Request/Response records
│   ├── Validators/                     # FluentValidation rules
│   └── Interfaces/                     # IPasswordHasher, IJwtTokenGenerator
│
├── MechanicalWorkshop.Infrastructure/  # EF Core, Repositories, Security
│   ├── Persistence/
│   │   ├── Context/                    # AppDbContext + DesignTimeFactory
│   │   ├── Mappings/                   # Fluent API configurations
│   │   ├── Repositories/              # Repository implementations
│   │   └── Migrations/                # EF Core migrations
│   └── Security/                       # PasswordHasher, JwtTokenGenerator
│
├── MechanicalWorkshop.API/             # Controllers, Middlewares, Extensions
│   ├── Controllers/                    # Auth, Customers, Vehicles, Services, Parts, ServiceOrders
│   ├── Middlewares/                    # ExceptionMiddleware, ValidationFilter
│   └── Extensions/                    # DI, JWT, Swagger, Database config
│
├── Dockerfile
├── docker-compose.yml
└── README.md
```

## Dependency Graph

```
API → Application → Domain
API → Infrastructure → Application → Domain
```

The Domain layer has zero external dependencies. Infrastructure implements the contracts defined by Domain. Application orchestrates domain logic. API is the entry point.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running
- (Optional) [.NET 10 SDK](https://dotnet.microsoft.com/download) for local development

## Getting Started

### Running with Docker (recommended)

```bash
git clone <repository-url>
cd tech-challenge

docker compose up --build
```

The API will be available at `http://localhost:8080`.

Swagger UI: `http://localhost:8080/swagger`

### Running Locally

```bash
# Ensure PostgreSQL is running on localhost:5432
# Update connection string in appsettings.json if needed

dotnet restore
dotnet build
dotnet run --project MechanicalWorkshop.API
```

## Authentication

The system uses JWT Bearer tokens for administrative endpoints. A default admin user is seeded on first run:

| Field | Value |
|-------|-------|
| Email | `admin@workshop.com` |
| Password | `Admin@123` |

### Login

```bash
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@workshop.com",
  "password": "Admin@123"
}
```

The response includes a JWT token. Use it in the `Authorization` header:

```
Authorization: Bearer <token>
```

In Swagger UI, click the "Authorize" button and paste the token.

## API Endpoints

### Auth
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/auth/login` | No | Authenticate and get JWT token |

### Customers
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/customers` | Yes | Create customer |
| GET | `/api/customers` | Yes | List all (optional `?name=` filter) |
| GET | `/api/customers/{id}` | Yes | Get by ID |
| GET | `/api/customers/document/{doc}` | Yes | Get by CPF/CNPJ |
| PUT | `/api/customers/{id}` | Yes | Update customer |
| DELETE | `/api/customers/{id}` | Yes | Delete customer |

### Vehicles
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/vehicles` | Yes | Create vehicle |
| GET | `/api/vehicles` | Yes | List all |
| GET | `/api/vehicles/{id}` | Yes | Get by ID |
| GET | `/api/vehicles/plate/{plate}` | Yes | Get by license plate |
| GET | `/api/vehicles/customer/{id}` | Yes | Get by customer |
| PUT | `/api/vehicles/{id}` | Yes | Update vehicle |
| DELETE | `/api/vehicles/{id}` | Yes | Delete vehicle |

### Services
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/services` | Yes | Create service |
| GET | `/api/services` | Yes | List all |
| GET | `/api/services/{id}` | Yes | Get by ID |
| GET | `/api/services/active` | Yes | List active only |
| PUT | `/api/services/{id}` | Yes | Update service |
| PATCH | `/api/services/{id}/activate` | Yes | Activate |
| PATCH | `/api/services/{id}/deactivate` | Yes | Deactivate |
| DELETE | `/api/services/{id}` | Yes | Delete service |

### Parts & Inventory
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/parts` | Yes | Create part |
| GET | `/api/parts` | Yes | List all |
| GET | `/api/parts/{id}` | Yes | Get by ID |
| GET | `/api/parts/active` | Yes | List active only |
| GET | `/api/parts/low-stock` | Yes | List low stock items |
| PUT | `/api/parts/{id}` | Yes | Update part info |
| PATCH | `/api/parts/{id}/add-stock` | Yes | Add stock quantity |
| PATCH | `/api/parts/{id}/remove-stock` | Yes | Remove stock quantity |
| PATCH | `/api/parts/{id}/activate` | Yes | Activate |
| PATCH | `/api/parts/{id}/deactivate` | Yes | Deactivate |
| DELETE | `/api/parts/{id}` | Yes | Delete part |

### Service Orders
| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| POST | `/api/service-orders` | Yes | Create order with items |
| GET | `/api/service-orders` | Yes | List all |
| GET | `/api/service-orders/{id}` | Yes | Get with full details |
| GET | `/api/service-orders/number/{num}` | **No** | Public: track by order number |
| GET | `/api/service-orders/customer/{id}` | Yes | Get by customer |
| GET | `/api/service-orders/status/{status}` | Yes | Filter by status |
| GET | `/api/service-orders/average-execution-time` | Yes | Average execution time |
| PATCH | `/api/service-orders/{id}/status` | Yes | Update status |
| POST | `/api/service-orders/{id}/items` | Yes | Add items to order |
| DELETE | `/api/service-orders/{id}` | Yes | Delete order |

## Service Order Status Flow

```
Received → InDiagnosis → WaitingApproval → InExecution → Finished → Delivered
```

Each transition is validated by the domain — invalid transitions (e.g., `Received → Finished`) are rejected automatically.

## Validation

All inputs are validated using FluentValidation:

- **CPF**: Full digit validation with check digits
- **CNPJ**: Full digit validation with check digits
- **License Plate**: Supports both old format (`ABC1234`) and Mercosul (`ABC1D23`)
- **Required fields**: Name, email, phone, prices, quantities
- **Business rules**: Unique documents, unique license plates, stock availability

## Database

PostgreSQL 16 was chosen for its robustness, open-source license, native UUID support, `ILike` for case-insensitive search, and excellent compatibility with EF Core.

Data is persisted via Docker volume (`postgres_data`). On first run, the system automatically applies migrations and seeds sample data (admin user, 8 services, 8 parts).

## Environment Variables

| Variable | Default | Description |
|----------|---------|-------------|
| `ConnectionStrings__DefaultConnection` | See docker-compose | PostgreSQL connection |
| `Jwt__Secret` | (configured) | JWT signing key |
| `Jwt__Issuer` | MechanicalWorkshop.API | Token issuer |
| `Jwt__Audience` | MechanicalWorkshop.Client | Token audience |
| `Jwt__ExpirationHours` | 8 | Token TTL |

## DDD Documentation

The full Domain-Driven Design documentation including Event Storming, aggregates, and ubiquitous language is available at:

**[Miro Board — Event Storming](https://miro.com/app/board/uXjVHdb4J3E=/?share_link_id=527909954955)**

## License

This project is part of the FIAP Pós Tech Software Architecture program — Phase 1 Tech Challenge.