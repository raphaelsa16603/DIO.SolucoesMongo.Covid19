# Projeto DadosCovi19BRTools
## Ferramenta para ler o arquivo (caso_full.csv.gz) de https://brasil.io/dataset/covid19/files/ 
## Descompactar o CSV, ler o CSV e Registrar em um banco de dodos SQLite local
## Atualizar base de dados de forma incremental por UF
### - Raphael Simões Andrade


### instalando o System.Configuration no projeto
### --
 - dotnet add package System.Configuration.ConfigurationManager --version 5.0.0

### Persistência no SQLite
### Patotes necessários
- dotnet add package Microsoft.EntityFrameworkCore.Tools
- dotnet add package Microsoft.EntityFrameworkCore.Sqlite

### Libs de Referência
- dotnet add reference ../LibConsoleProgressBar/
- dotnet add reference ../LibFileDownload/
- dotnet add reference ../LibFileTools/
- dotnet add reference ../LibToolsLog/
- dotnet add reference ../LibWeb/

