# \# Meeting Scheduler API

# 

# \## Descrição

# 

# A Meeting Scheduler API é uma API RESTful desenvolvida em C# com ASP.NET Core e Entity Framework Core para gerenciar o agendamento de reuniões em salas. Utiliza SQLite como banco de dados e é estruturada seguindo boas práticas de arquitetura em camadas, com separação clara entre `Domain`, `Data` e `API`.

# 

# \## Como Executar com Docker Compose

# 

# \### Pré-requisitos

# 

# \- Docker

# \- Docker Compose

# 

# \### Configuração e Execução

# 

# 1\. \*\*Clone o repositório\*\* (se aplicável).

# 2\. \*\*Configure a string de conexão\*\*: Certifique-se de que a string de conexão no `appsettings.json` esteja configurada para usar SQLite ou outro banco de dados de sua escolha.

# 3\. \*\*Execute o Docker Compose\*\*:

# 

# &nbsp;   ```shell

# &nbsp;   docker-compose up --build

# &nbsp;   ```

# 

# &nbsp;  Este comando constrói e inicia os serviços definidos no `docker-compose.yml`, incluindo a API e o banco de dados.

# 

# \### docker-compose.yml

# 

# ```yaml

# version: '3.4'

# 

# services:

# &nbsp; meetingschedulerapi:

# &nbsp;   image: meetingschedulerapi

# &nbsp;   build:

# &nbsp;     context: .

# &nbsp;     dockerfile: Dockerfile

# &nbsp;   ports:

# &nbsp;     - "5000:8080"

# 

# &nbsp;   volumes:

# &nbsp;     - type: bind

# &nbsp;       source: .\\meetingScheduler.db

# &nbsp;       target: \\app\\meetingScheduler.db

# ```

# 

# \## Trade-offs Importantes (ADRs)

# 

# \### Arquitetura em Camadas

# 

# \- \*\*Separacao de Responsabilidades\*\*: Mantém a lógica de negócios (`Domain`), acesso a dados (`Data`) e a interface de API (`API`) separados, facilitando a manutenção e testabilidade.

# \- \*\*Complexidade\*\*: Aumenta a complexidade inicial do projeto, mas proporciona escalabilidade e clareza a longo prazo.

# 

# \### SQLite vs. Outros Bancos de Dados

# 

# \- \*\*Simplicidade\*\*: SQLite é fácil de configurar e usar, ideal para projetos de pequeno a médio porte ou ambientes de desenvolvimento.

# \- \*\*Escalabilidade\*\*: Bancos de dados como PostgreSQL ou SQL Server oferecem melhor escalabilidade e recursos adicionais para ambientes de produção.

# 

# \### Injeção de Dependência e Mocking

# 

# \- \*\*Testabilidade\*\*: Utiliza injeção de dependência e mocking (com Moq e NUnit ou xUnit) para testar a lógica de negócios sem depender de um banco de dados real.

# \- \*\*Complexidade\*\*: Requer um entendimento de injeção de dependência e testes unitários.

# 

# \### Uso de entidades como DTOs

# 

# \- \*\*Agilidade na entrega\*\*: possibilita a escrita de menos códigos e a criação de menos estruturas de dados agilizando a entrega do teste de proficiência.

# \- \*\*Queda de performance\*\*: a ativação da configuração para ignorar ciclos de referência pode gerar queda siginificativa na performance de vários endpoints.

# 

# \## Possíveis Melhorias

# 

# \- \*\*Autenticação e Autorização\*\*: Implementar JWT ou OAuth para segurança adicional.

# \- \*\*Logging e Monitoramento\*\*: Adicionar serviços de logging (como Serilog) e monitoramento (como Prometheus e Grafana).

# \- \*\*API Versioning\*\*: Implementar versionamento de API para gerenciar mudanças futuras na API de forma mais organizada.

# \- \*\*Caching\*\*: Utilizar caching para melhorar a performance, especialmente em consultas frequentes.

# \- \*\*Health Checks\*\*: Adicionar verificações de saúde para monitorar o status do banco de dados e da API.

# \- \*\*Documentação Swagger\*\*: Enriquecer a documentação Swagger com mais exemplos e descrições.

# \- \*\*Adição de mais DTOs\*\*: como exposto anteriormente, foram usados modelos de entidade em vários pontos do código e ativada a configuração para ignorar ciclos de referência no serializador Json, porém, em um projeto aplicado a produção isso não seria uma boa descisão, sendo necessário criar mais DTOs para utilizar como I/O de vários métodos.

# \- \*\*Adição de mais casos de teste\*\*: seria necessária a crição de mais casos de teste para abarcar toda a extensão do código. 

