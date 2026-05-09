
# Banco.API — Banco Digital com Mensageria RabbitMQ

Projeto desenvolvido para a disciplina de Engenharia de Software — FIAP 3ESR 2026.  
O sistema simula o backend de um banco digital utilizando .NET 8, Oracle, RabbitMQ, Entity Framework Core, Serilog, OpenTelemetry e testes automatizados.

---

# 1. Identificação

| Nome | RM |
|---|---|
| Jhonatan Quispe Torrez | RM506060 |
| Julia Damasceno Busso | RM560293 |
| Gabriel Gomes Cardoso | RM559597 |

---

# 2. Produto Bancário Escolhido e Justificativa

## Produto Escolhido
### Empréstimo

O produto bancário implementado foi **Empréstimo**.

A escolha foi feita porque esse tipo de produto possui um fluxo extremamente compatível com processamento assíncrono, análise de crédito e validação posterior.

Durante uma solicitação de empréstimo normalmente existem etapas que não devem bloquear a resposta da API, como:

- análise de crédito;
- validação de cliente;
- cálculo de risco;
- processamento financeiro;
- aprovação posterior.

Por esse motivo, o fluxo utilizando RabbitMQ se encaixa perfeitamente no cenário proposto pela atividade.

---

# 3. Decisão de Modelagem de Filas

## Estratégia Escolhida
### Uma fila central de contratações

Fila utilizada:

contratacao-solicitada


A equipe optou por utilizar **uma única fila central** para processamento das contratações.

## Justificativa

A escolha foi feita considerando os seguintes trade-offs:

### Vantagens

* menor complexidade inicial;
* manutenção mais simples;
* gerenciamento centralizado;
* menor quantidade de configurações;
* mais fácil de debugar;
* ideal para um único produto implementado.

### Desvantagens

* menor escalabilidade futura;
* todos os produtos compartilham o mesmo consumer;
* necessidade de discriminator caso existam múltiplos produtos futuramente.

## Fluxo Assíncrono

```txt
Cliente faz requisição
        ↓
API valida cliente
        ↓
Contratação salva como PENDENTE
        ↓
Mensagem publicada no RabbitMQ
        ↓
Consumer recebe mensagem
        ↓
Processamento assíncrono
        ↓
Status atualizado para APROVADA
```

---

# 4. Diagrama de Classes

## Diagrama UML


<details>
  <summary>📘 Banco.API - Diagrama de Classes UML</summary>

  ![DIAGRAMA](https://github.com/JhowQT/Banco/issues/1#issue-4410403746)

  _Figura: MER do sistema._
</details>

---

# Estrutura das Entidades

## Agência

Representa uma agência bancária.

### Campos

* Id
* Nome

### Relacionamentos

* Uma agência possui vários clientes.

---

## Cliente (Classe Abstrata)

Classe base do sistema.

### Campos

* Id
* AgenciaId

### Relacionamentos

* Cliente pertence a uma agência;
* Cliente possui várias contratações.

### Heranças

* PessoaFisica
* PessoaJuridica

---

## PessoaFisica

Herda de Cliente.

### Campos

* CPF
* DataNascimento

### Regras

* CPF não pode duplicar.

---

## PessoaJuridica

Herda de Cliente.

### Campos

* CNPJ
* RazaoSocial

### Regras

* CNPJ não pode duplicar.

---

## Produto

Classe abstrata base dos produtos bancários.

### Campos

* Id
* Nome

---

## Empréstimo

Herda de Produto.

### Campos

* Valor
* TaxaJuros

---

## Contratação

Representa uma contratação bancária.

### Campos

* Id
* ClienteId
* ProdutoId
* Status
* DataCriacao

### Fluxo

* cria com status `PENDENTE`;
* publica mensagem na fila;
* consumer processa;
* status atualizado para `APROVADA`.

---

# 5. Como Rodar Localmente

## Pré-requisitos

Instalar:

* .NET 8 SDK
* Docker Desktop
* Visual Studio 2022 ou VS Code

---

# RabbitMQ via Docker

## Executar Container

```bash
docker run -d --hostname rabbit-host --name rabbitmq ^
-p 5672:5672 ^
-p 15672:15672 ^
rabbitmq:3-management
```

---

# Painel RabbitMQ

## URL

```txt
http://localhost:15672
```

## Usuário

```txt
guest
```

## Senha

```txt
guest
```

---

# Configuração Oracle

## appsettings.json

```json
"ConnectionStrings": {
  "Oracle": "User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
}
```

---

# Executar Projeto

## 1. Clonar repositório

```bash
git clone URL_DO_REPOSITORIO
```

---

## 2. Restaurar pacotes

```bash
dotnet restore
```

---

## 3. Aplicar migrations

```bash
dotnet ef database update
```

---

## 4. Rodar API

```bash
dotnet run
```

---

# Swagger

## URL

```txt
https://localhost:7262/swagger
```

---

# 6. Endpoints Disponíveis

# Agências

## POST — Criar Agência

### Endpoint

```http
POST /api/agencias
```

### Request

```json
{
  "nome": "Agencia Paulista"
}
```

### Response

```json
{
  "id": 1,
  "nome": "Agencia Paulista"
}
```

---

## GET — Buscar Todas

### Endpoint

```http
GET /api/agencias
```

### Response

```json
[
  {
    "id": 1,
    "nome": "Agencia Paulista"
  }
]
```

---

## GET — Buscar Agência por ID

### Endpoint

```http
GET /api/agencias/1
```

### Response

```json
{
  "id": 1,
  "nome": "Agencia Paulista"
}
```

---

## PUT — Atualizar Agência

### Endpoint

```http
PUT /api/agencias/1
```

### Request

```json
{
  "nome": "Agencia Atualizada"
}
```

### Response

```json
{
  "id": 1,
  "nome": "Agencia Atualizada"
}
```

---

## DELETE — Deletar Agência

### Endpoint

```http
DELETE /api/agencias/1
```

---

# Clientes PF

## POST — Criar Pessoa Física

### Endpoint

```http
POST /api/clientes/pf
```

### Request

```json
{
  "cpf": "12345678900",
  "dataNascimento": "2000-05-10",
  "agenciaId": 1
}
```

### Response

```json
{
  "id": 1,
  "agenciaId": 1,
  "tipo": "PF",
  "cpf": "12345678900",
  "dataNascimento": "2000-05-10T00:00:00"
}
```

---

# Clientes PJ

## POST — Criar Pessoa Jurídica

### Endpoint

```http
POST /api/clientes/pj
```

### Request

```json
{
  "cnpj": "12345678000199",
  "razaoSocial": "Empresa XPTO",
  "agenciaId": 1
}
```

### Response

```json
{
  "id": 2,
  "agenciaId": 1,
  "tipo": "PJ",
  "cnpj": "12345678000199",
  "razaoSocial": "Empresa XPTO"
}
```

---

# Contratações

## POST — Solicitar Contratação

### Endpoint

```http
POST /api/contratacoes
```

### Request

```json
{
  "clienteId": 1,
  "produtoId": 1
}
```

### Response Inicial

```json
{
  "id": 1,
  "clienteId": 1,
  "produtoId": 1,
  "status": "PENDENTE",
  "dataCriacao": "2026-05-08T20:00:00"
}
```

## Funcionamento

* contratação salva como `PENDENTE`;
* mensagem publicada no RabbitMQ;
* processamento assíncrono;
* consumer atualiza para `APROVADA`.

---

## GET — Buscar Contratação por ID

### Endpoint

```http
GET /api/contratacoes/1
```

### Response Após Processamento

```json
{
  "id": 1,
  "clienteId": 1,
  "produtoId": 1,
  "status": "APROVADA",
  "dataCriacao": "2026-05-08T20:00:00"
}
```

---

# Health Check

## Endpoint

```http
GET /health
```

---

# 7. Como Executar os Testes

## Executar Testes

```bash
dotnet test
```

---

# Resultado Esperado

```txt
Passed! 3 tests passed.
0 failed.
```

---

# Print dos Testes

Adicionar imagem em:

```txt
/docs/testes-aprovados.png
```

Exemplo:

```md
![Testes](docs/testes-aprovados.png)
```

---

# 8. Print do Painel RabbitMQ

Adicionar imagem em:

```txt
/docs/rabbitmq-fila.png
```

A imagem deve mostrar:

* fila criada;
* mensagens processadas;
* mensagens ACK;
* Unacked (quando aplicável).

Exemplo:

```md
![RabbitMQ](docs/rabbitmq-fila.png)
```

---

# 9. Print da API Rodando no Swagger

Adicionar imagem em:

```txt
/docs/swagger-contratacao.png
```

A imagem deve mostrar:

* Swagger funcionando;
* endpoint de contratação;
* contratação com status `APROVADA`.

Exemplo:

```md
![Swagger](docs/swagger-contratacao.png)
```

---

# Observabilidade

## Serilog

Logs implementados em:

* console;
* arquivo.

Pasta:

```txt
Logs/
```

---

## OpenTelemetry

Tracing HTTP implementado para:

* requests;
* status code;
* tempo de execução.

Exporter utilizado:

```txt
Console
```

---

# Estrutura do Projeto

```txt
Banco.API
│
├── Controllers
├── Application
│     ├── DTOs
│     └── Services
│
├── Domain
│     └── Entities
│
├── Infrastructure
│     ├── Data
│     ├── Messaging
│     └── Repositories
│
├── BackgroundServices
│
├── Tests
│
└── Migrations
```

---

# Considerações Finais

O projeto implementa:

* arquitetura em camadas;
* mensageria assíncrona;
* RabbitMQ com ACK manual;
* persistência Oracle;
* Entity Framework Core;
* testes automatizados;
* observabilidade;
* Health Checks;
* boas práticas REST.

A solução foi desenvolvida conforme os requisitos obrigatórios definidos no enunciado da atividade.

```
```
