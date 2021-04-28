# Projeto DadosCovi19BRTools
## Ferramenta para ler o arquivo (caso_full.csv.gz) de https://brasil.io/dataset/covid19/files/ 
## Descompactar o CSV, ler o CSV e Registrar em um banco de dodos SQLite local
### - Raphael Simões Andrade


### instalando o System.Configuration no projeto
### --
 - dotnet add package System.Configuration.ConfigurationManager --version 5.0.0

### Persistência no PostgreSQL
### Patotes necessários
- dotnet add package Microsoft.EntityFrameworkCore.Tools
- dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL 

## EF Criação da base de dados
- dotnet ef migrations add InitDadosCovid19BR
- dotnet ef database update


