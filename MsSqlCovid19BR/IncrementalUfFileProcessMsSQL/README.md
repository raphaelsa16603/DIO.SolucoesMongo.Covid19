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
- dotnet add package Microsoft.EntityFrameworkCore.SqlServer

Server=.;Database=Covid19Br;userid=appcovid19br;password=covid19BR;



