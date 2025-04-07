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
## Instruções de execução

🔧 Certifique-se de que o SDK do .NET 8 esteja instalado no seu sistema. Como o projeto é multiplataforma 🌐, você poderá executá-lo tranquilamente em qualquer ambiente: seja no Windows 🪟, Linux 🐧 ou macOS 🍎.

🧪➡️ dotnet run ou dotnet watch run 

🛠️ Caso queira utilizar um banco de dados diferente seja local 🖥️ ou em nuvem ☁️ , basta ajustar a string de conexão no arquivo appsettings.Development.json, que fica no caminho:

📁 uCondoHandsOn.API/appsettings.Development.json

Lá dentro, encontre o bloco:

"ConnectionStrings": {
  "Connections": "sua_connection_string"
}

🔄 Substitua "sua_connection_string" pela nova configuração do seu banco. Depois disso, é só rodar o projeto normalmente!

🧩 O projeto foi construído com o EntityFrameworkCore , e as migrações são aplicadas de forma automática ⚙️. Isso quer dizer que, ao atualizar a connection string e executar a aplicação ▶️, todas as tabelas e estruturas do banco de dados serão criadas automaticamente 🛠️ sem que você precise rodar comandos manuais ou realizar configurações adicionais. Simples assim!

## Testes unitários

O projeto conta com testes unitários, que podem ser executados de forma simples tanto por uma IDE de preferência 🧠💻 quanto via terminal 🖥️, usando o comando abaixo:

🧪➡️ dotnet test

🚀 Assim que o projeto for iniciado, ele estará pronto para receber requisições HTTP em um servidor local no seguinte endereço:

🌐 Base URL:

https://localhost:5001/swagger/index.html

📌 Endpoints disponíveis:

📄 Listar plano de contas:
GET https://localhost:5001/api/accounts

➕ Criar nova conta:
POST https://localhost:5001/api/accounts

🔢 Obter sugestão de código para próximo filho:
GET https://localhost:5001/api/accounts/<code>/next

❌ Excluir conta (e seus filhos):
DELETE https://localhost:5001/api/accounts/<code>





