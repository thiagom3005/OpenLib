# OpenLib - Gestão de Bibliotecas Comunitárias

Projeto de referência para um back-end em **.NET 8** que aplica conceitos de *Clean Architecture* e domínio rico para gerenciar livros e empréstimos em bibliotecas comunitárias.

## 🏗️ Arquitetura

A solução está organizada nas seguintes camadas:

- **Domain**: Entidades ricas (`Livro`, `Emprestimo`), validações com FluentValidation e regras de negócio.
- **Application**: Serviços de aplicação, DTOs, contratos de repositório e Unit of Work.
- **Infrastructure**: Implementação de persistência com Entity Framework Core e PostgreSQL, repositórios concretos e configuração de DI.
- **API**: Endpoints RESTful para interação com o domínio.

```
OpenLib.sln
├── src/
│   ├── OpenLib.Domain
│   ├── OpenLib.Application
│   ├── OpenLib.Infrastructure
│   └── OpenLib.Api
└── tests/
    └── OpenLib.UnitTests
```

## 🚀 Requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/)
- Banco de dados PostgreSQL

## ⚙️ Configuração

1. **Clonar o repositório**

   ```bash
   git clone <url-do-repositorio>
   cd OpenLib
   ```

2. **Configurar string de conexão**

   Ajuste `ConnectionStrings:DefaultConnection` em `src/OpenLib.Api/appsettings.json` com as credenciais do seu PostgreSQL.

3. **Restaurar dependências**

   ```bash
   dotnet restore
   ```

4. **Executar a API**

   ```bash
   dotnet run --project src/OpenLib.Api/OpenLib.Api.csproj
   ```

   A API é inicializada com Swagger em `https://localhost:5001/swagger` (ou porta configurada). O contexto aplica migrações disponíveis ou cria o schema inicial automaticamente.

## 📚 Endpoints Principais

| Método | Rota | Descrição |
| ------ | ---- | --------- |
| `POST` | `/api/livros` | Cria um novo livro |
| `GET` | `/api/livros/{id}` | Obtém livro por identificador |
| `GET` | `/api/livros` | Lista livros cadastrados |
| `POST` | `/api/emprestimos` | Solicita um empréstimo |
| `POST` | `/api/emprestimos/{id}/devolver` | Realiza a devolução do empréstimo |
| `GET` | `/api/emprestimos/{id}` | Obtém empréstimo por identificador |
| `GET` | `/api/emprestimos` | Lista empréstimos |

## ✅ Testes

Os testes automatizados validam as regras de domínio e os serviços de aplicação.

```bash
 dotnet test
```

## 🧩 Padrões e Boas Práticas

- Domínio rico com validações através do FluentValidation.
- Separação clara de responsabilidades entre camadas.
- Unit of Work para consistência das operações de escrita.
- Repositórios e serviços de aplicação assíncronos.

## 📄 Licença

Projeto criado para fins educacionais.
