# Projeto DadosCovi19BRTools
## Ferramenta para ler o arquivo (caso_full.csv.gz) de https://brasil.io/dataset/covid19/files/ 
## Descompactar o CSV, ler o CSV e Registrar em um banco de dodos MongoDB no Atlas
### - Raphael Simões Andrade

### Visual Studio -- launch.json
- "console": "integratedTerminal",

### instalando o System.Configuration no projeto
### --
- dotnet add package System.Configuration.ConfigurationManager --version 5.0.0

#### https://www.nuget.org/packages/mongodb.driver
- dotnet add package MongoDB.Driver --version 2.12.2
- dotnet add package MongoDB.Driver 

### Libs de Referência
- dotnet add reference ../LibConsoleProgressBar/
- dotnet add reference ../LibFileDownload/
- dotnet add reference ../LibFileTools/
- dotnet add reference ../LibToolsLog/
- dotnet add reference ../LibWeb/
