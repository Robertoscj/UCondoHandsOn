# API de Plano de Contas uCondo

## Visão Geral
Esta API fornece funcionalidades para gerenciamento de planos de contas no sistema uCondo, incluindo criação, leitura e exclusão de contas, com suporte para estruturas hierárquicas e regras de validação. O projeto foi desenvolvido utilizando .NET 8 e segue os princípios de Clean Architecture.

## Tecnologias Utilizadas
- .NET 8
- Entity Framework Core 8.0
- SQL Server
- Swagger/OpenAPI
- Mapper
- JWT e OAuth2 para autenticação
- xUnit para testes

## Pré-requisitos
- .NET 8 SDK
- SQL Server (2019 ou superior)
- Visual Studio 2022 ou VS Code
- Docker (opcional)

## Estrutura do Projeto
```plaintext
├── src/
│   ├── uCondoHandsOn.API/           # Projeto da API
│   ├── uCondoHandsOn.Application/   # Casos de uso e DTOs       
│   ├── uCondoHandsOn.Domain/        # Regras de negócio e entidades
│   └── uCondoHandsOn.Infrastructure/# Acesso a dados e serviços externos   
└── tests/
    └── uCondoHandsOn.Tests/         # Testes unitários e de integração
```

## Como Começar

### 1. Clonar o Repositório
```bash
git clone https://github.com/Robertoscj/UCondoHandsOn.git 
cd UCondoHandsOn
```
