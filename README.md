# Todo Project 

## Get Started

```bash
docker-compose up -d postgresql
docker-compose up -d rabbitmq
docker-compose up -d --build api
docker-compose up -d --build ui.blazor
docker-compose up -d postgresql-exporter
docker-compose up -d prometheus
docker-compose up -d grafana
```


### Swagger API

<http://localhost:6001/swagger/index.html>


### RabbitMQ

<http://localhost:15672>


### Blazor UI

<http://localhost:6002>


### Prometheus

<http://localhost:9090>

Exemplos de queries:  
<https://prometheus.io/docs/prometheus/latest/querying/examples/>


### Grafana

<http://localhost:3000>  
Entrar com usuário e senha: admin

Pesquisar dashboards prontos:  
<https://grafana.com/grafana/dashboards/>

Sugestão de dashboard: 9628


<br>


## Migrations

```bash
dotnet ef migrations add <migrationName> -s src/Todo.Api -p src/Todo.Infrastructure
dotnet ef database update <migrationName> -s src/Todo.Api -p src/Todo.Infrastructure
dotnet ef migrations remove -s src/Todo.Api -p src/Todo.Infrastructure
```


<br>


## References

<https://www.youtube.com/watch?v=NegCR6J2eeE>
