## 
## Link para baixar o DataSet: https://brasil.io/dataset/covid19/files/

## Gerando executável para linux e Windows
- dotnet publish -c Debug -r linux-x64 -p:PublishSingleFile=True --self-contained True
- dotnet publish -c Debug -r win10-x64 -p:PublishSingleFile=True --self-contained True
- dotnet publish -c Release -r osx-x64 -p:PublishSingleFile=True --self-contained True
