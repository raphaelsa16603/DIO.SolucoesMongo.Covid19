﻿using System;

namespace CovidBrDataSetFileProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Ler a página https://brasil.io/dataset/covid19/files/ 
            // e pegar a data do arquivo -- Classe Ler dados da página!
            
            // TODO: Baixar o arquivo .gzip -- Classe para baixar arquivo da web

            // TODO: Descompactar o arquivo, colocando em uma pasta temporária
            // -- Classe para descompactar arquivo

            // TODO: Renomear o arquivo para colocar a data de processamento contida 
            // na página no nome do arquivo ... tipo 2021-04-20 - [nome do arquivo.csv]
            // -- Classe para tratar arquivo

            // TODO: Ler o arquivo csv linha por linha e colocar no banco de dados local
            // Banco de dados SQLite!
            // -- Classe para ler arquivo texto ou csv
            // -- Classe para ler dados do arquivo csv, 
            // pegando o cabeçalho e lendo coluna por coluna e linha por linha
            // -- Classe do EntityFramework para salvar os dados lidos e colocar na base de dados
            // -- Classe e Domain / Controller para verificar registro por registro do csv 
            // para ver se os dados do arquivo já não estão armazenados no banco de dados 
            // e se tiver, verificar se há atualização e atualiza o registro no BD, sentando a flag
            // atualizado. 
        }
    }
}
