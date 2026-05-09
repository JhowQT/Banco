Banco.API — Banco Digital com Mensageria RabbitMQ

Projeto desenvolvido para a disciplina de Engenharia de Software — FIAP 3ESR 2026.
O sistema simula o backend de um banco digital utilizando .NET 8, Oracle, RabbitMQ, Serilog, OpenTelemetry e testes automatizados.

👨‍💻 Integrantes
Nome	RM
Jhonatan Quispe Torrez	RM XXXXX
Integrante 2	RM XXXXX
Integrante 3	RM XXXXX
📚 Tecnologias Utilizadas
Tecnologia	Função
.NET 8	Runtime principal
ASP.NET Core Web API	API REST
Entity Framework Core	ORM
Oracle Database	Banco de dados
RabbitMQ	Mensageria
Docker	Container RabbitMQ
Serilog	Logs estruturados
OpenTelemetry	Observabilidade
Health Checks	Monitoramento da API
xUnit	Testes automatizados
Moq	Mock de testes
WebApplicationFactory	Testes de integração
🏦 Produto Bancário Escolhido

O produto implementado foi:

EMPRÉSTIMO

A escolha foi feita porque o produto possui um fluxo muito compatível com processamento assíncrono, análise de crédito e aprovação posterior.

📨 Estratégia de Mensageria

O projeto utiliza:

UMA FILA CENTRAL

Fila utilizada:

contratacao-solicitada

A decisão foi tomada porque:

simplifica o gerenciamento
facilita manutenção
centraliza processamento
reduz complexidade inicial

O processamento ocorre de forma assíncrona:

API recebe contratação
Contratação é salva como PENDENTE
Mensagem é enviada ao RabbitMQ
Consumer processa
Status muda para APROVADA

Implementação baseada nos requisitos do PDF da atividade.

🧱 Arquitetura do Projeto

O projeto foi dividido em camadas:

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
└── Migrations
📌 Modelagem de Objetos

A modelagem segue exatamente os requisitos da atividade.

🏢 Agencia

Representa a agência bancária.

Campos:

Id
Nome

Relacionamentos:

Uma agência possui vários clientes

👤 Cliente (Classe Abstrata)

Classe base do sistema.

Campos:

Id
AgenciaId

Relacionamentos:

Cliente pertence a uma agência
Cliente possui várias contratações

Heranças:

PessoaFisica
PessoaJuridica

👨 PessoaFisica

Herda de Cliente.

Campos:

CPF
DataNascimento

Validações:

CPF não pode duplicar
🏢 PessoaJuridica

Herda de Cliente.

Campos:

CNPJ
RazaoSocial

Validações:

CNPJ não pode duplicar
💰 Produto

Classe abstrata base dos produtos bancários.

Campos:

Id
Nome

💵 Emprestimo

Herda de Produto.

Campos:

Valor
TaxaJuros

📄 Contratacao

Representa a contratação de um produto bancário.

Campos:

Id
ClienteId
ProdutoId
Status
DataCriacao

Fluxo:

Cria PENDENTE
Publica na fila
Consumer processa
Atualiza para APROVADA
🔄 Fluxo Completo da Contratação
Cliente faz requisição
        ↓
API valida cliente
        ↓
Contratação salva como PENDENTE
        ↓
Mensagem enviada ao RabbitMQ
        ↓
Consumer recebe mensagem
        ↓
Processamento assíncrono
        ↓
Status atualizado para APROVADA
🐇 RabbitMQ

O projeto utiliza:

RabbitMQ Publisher
RabbitMQ Consumer
ACK manual
fila persistente

O Consumer roda em:

BackgroundService

🩺 Health Check

Endpoint:

GET /health

Valida:

API funcionando
conexão Oracle ativa

Implementado utilizando:

AddDbContextCheck<AppDbContext>

📊 Observabilidade
✅ Serilog

Logs:

console
arquivo

Pasta:

Logs/
✅ OpenTelemetry

Tracing HTTP:

requests
status code
tempo de execução

Exporter:

Console
🧪 Testes Automatizados

O projeto possui:

✅ Testes Unitários
CPF duplicado
Agência inexistente
✅ Testes de Integração
acesso Swagger
WebApplicationFactory
🐳 Como Rodar o RabbitMQ no Docker
Executar container
docker run -d --hostname rabbit-host --name rabbitmq ^
-p 5672:5672 ^
-p 15672:15672 ^
rabbitmq:3-management
🔑 Acessar painel RabbitMQ

URL:

http://localhost:15672

Usuário:

guest

Senha:

guest
🗄️ Banco Oracle

Connection String utilizada:

"ConnectionStrings": {
  "Oracle": "User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
}
▶️ Como Rodar o Projeto
1. Clonar repositório
git clone URL_DO_REPOSITORIO
2. Restaurar pacotes
dotnet restore
3. Aplicar migrations
dotnet ef database update
4. Rodar API
dotnet run
🌐 Swagger

URL:

https://localhost:7262/swagger
📌 Endpoints Disponíveis

Todos os endpoints seguem os requisitos mínimos da atividade.

🏢 Agências
GET — Buscar Todas
GET /api/agencias
GET — Buscar por ID
GET /api/agencias/{id}

Exemplo:

GET /api/agencias/1
POST — Criar Agência
POST /api/agencias

JSON:

{
  "nome": "Agencia Paulista"
}
PUT — Atualizar Agência
PUT /api/agencias/{id}

JSON:

{
  "nome": "Agencia Atualizada"
}
DELETE — Deletar Agência
DELETE /api/agencias/{id}
👤 Clientes PF
POST — Criar Pessoa Física
POST /api/clientes/pf

JSON:

{
  "cpf": "12345678900",
  "dataNascimento": "2000-05-10",
  "agenciaId": 1
}
PUT — Atualizar PF
PUT /api/clientes/pf/{id}

JSON:

{
  "cpf": "99999999999",
  "dataNascimento": "1999-10-10",
  "agenciaId": 1
}
🏢 Clientes PJ
POST — Criar Pessoa Jurídica
POST /api/clientes/pj

JSON:

{
  "cnpj": "12345678000199",
  "razaoSocial": "Empresa XPTO",
  "agenciaId": 1
}
PUT — Atualizar PJ
PUT /api/clientes/pj/{id}

JSON:

{
  "cnpj": "99999999000199",
  "razaoSocial": "Nova Empresa",
  "agenciaId": 1
}
👥 Clientes Gerais
GET — Buscar Todos
GET /api/clientes
GET — Buscar por ID
GET /api/clientes/{id}
DELETE — Deletar Cliente
DELETE /api/clientes/{id}
📄 Contratações
GET — Buscar Todas
GET /api/contratacoes
GET — Buscar por ID
GET /api/contratacoes/{id}
POST — Criar Contratação
POST /api/contratacoes

JSON:

{
  "clienteId": 1,
  "produtoId": 1
}

Resultado:

status inicial = PENDENTE
mensagem publicada no RabbitMQ
processamento assíncrono
PUT — Atualizar Contratação
PUT /api/contratacoes/{id}

JSON:

{
  "clienteId": 1,
  "produtoId": 1,
  "status": "APROVADA"
}
DELETE — Deletar Contratação
DELETE /api/contratacoes/{id}
🧪 Como Executar os Testes
Visual Studio
Teste → Executar Todos os Testes
Terminal
dotnet test
✅ Resultado Esperado
3 testes aprovados
0 falhas
📸 Prints Obrigatórios

Adicionar no repositório:

Swagger funcionando
RabbitMQ processando mensagens
Health Check Healthy
Logs do Serilog
OpenTelemetry no console
Testes verdes
📌 Considerações Finais

O projeto implementa:

arquitetura em camadas
mensageria assíncrona
observabilidade
testes automatizados
persistência Oracle
boas práticas REST
