using System;
using CovidBrDataSetFileProcess.Lib.ProgressBar;
using System.Threading;
using CovidBrDataSetFileProcess.Lib.DownLoadFile;
using CovidBrDataSetFileProcess.Web;
using System.Configuration;
using CovidBrDataSetFileProcess.Lib.FileTools;
using System.IO;
using CovidBrDataSetFileProcess.Business;

namespace CovidBrDataSetFileProcess
{
    class Program
    {
        
        
        static void Main(string[] args)
        {
            // TODO: Ler a página https://brasil.io/dataset/covid19/files/ 
            // e pegar a data do arquivo -- Classe Ler dados da página!
            string Data = LoadInfoUrl.GetDataUrlCovid19BrFiles();
            System.Console.WriteLine($"Data do arquivo {Data}");
            
            // TODO: -- Classe para fazer barra de progresso na tela do console
            TesteDoProgressBar();

            string diretorioDataSet = ConfigurationManager.AppSettings["dir"];
            string arquivoDB = ConfigurationManager.AppSettings["file"];
            string pathString = System.IO.Path.Combine(diretorioDataSet, arquivoDB);

            DirectoryInfo directoryData = new DirectoryInfo(diretorioDataSet);
            bool existeArquivoCsvDoDia = false;
            foreach (FileInfo fileToCsv in directoryData.GetFiles("*.csv"))
            {
                existeArquivoCsvDoDia = 
                    fileToCsv.FullName.Contains(Data.Trim().Replace("/","-"));
            }
            if(!existeArquivoCsvDoDia)
            {
                // TODO: Baixar o arquivo .gzip -- Classe para baixar arquivo da web
                // Statico === 
                //DownLoadFile.DownLoadFileInBackground4(@"https://data.brasil.io/dataset/covid19/caso_full.csv.gz");
                DownLoadFile DownPB = new DownLoadFile();
                DownPB.DownLoadFileInBackgroundByProgBar4
                    (@"https://data.brasil.io/dataset/covid19/caso_full.csv.gz");

                // TODO: Descompactar o arquivo, colocando em uma pasta temporária
                // -- Classe para descompactar arquivo
                CompactacaoArquivo.Decompress(pathString);


                // TODO: Renomear o arquivo para colocar a data de processamento contida 
                // na página no nome do arquivo ... tipo 2021-04-20 - [nome do arquivo.csv]
                // -- Classe para tratar arquivo ???
                DirectoryInfo directorySelected = new DirectoryInfo(diretorioDataSet);
                foreach (FileInfo fileToCsv in directorySelected.GetFiles("*.csv"))
                {
                    string extensao = fileToCsv.Extension;
                    string nome = fileToCsv.Name;
                    long tamanho = fileToCsv.Length;
                    string NovoNomeArquivo = System.IO.Path.Combine(
                            fileToCsv.Directory.FullName, 
                            Data.Trim().Replace("/","-") + " - " + nome);
                    System.Console.WriteLine(NovoNomeArquivo);
                    fileToCsv.MoveTo(NovoNomeArquivo);
                }
            }
            
            // -- Devido ao problemas com o download do arquivo a parte de leitura
            // -- do arquivo csv a atualização no Banco de Dados será feito separadamente

            // TODO: Ler o arquivo csv linha por linha e colocar no banco de dados local
            // Banco de dados SQLite!
            // -- Classe para ler arquivo texto ou csv
            System.Console.WriteLine("----------------------------------------------");
            DirectoryInfo directoryFilesCsv = new DirectoryInfo(diretorioDataSet);
            var ForDb = new RegistroDeDadosDbLocal();
            foreach (FileInfo fileToCsv in directoryFilesCsv.GetFiles("*.csv"))
            {
                ReadingCSV.LerArquivoCsv(fileToCsv.FullName, ForDb.processarArqCsvInserirNoDB);
            }
            
            // -- Classe para ler dados do arquivo csv, 
            // pegando o cabeçalho e lendo coluna por coluna e linha por linha
            // -- Classe do EntityFramework para salvar os dados lidos e colocar na base de dados
            // -- Classe e Domain / Controller para verificar registro por registro do csv 
            // para ver se os dados do arquivo já não estão armazenados no banco de dados, 
            // se não tiver, insere o registo com a fleg novo e o uId gerado, 
            // e se tiver, verificar se há atualização e atualiza o registro no BD, sentando a flag
            // atualizado, mantendo o uId original. 
        }

        static void processarArqCsv( string [] listaCampos, long contador, long totallinhas)  
        {
            foreach (string campo in listaCampos)
            {
                System.Console.Write(" [" + campo + "]");
            }
            System.Console.WriteLine( " - Linha[" + 
                contador + "] do total [" + totallinhas + "]");
        }

        static void processarArqCsvBarraProgresso( string [] listaCampos, long contador, long totallinhas)  
        {
            foreach (string campo in listaCampos)
            {
                //System.Console.Write(" [" + campo + "]");
            }
            ToolsProgressBar tools = new ToolsProgressBar();
            int percentagem = (int)Math.Round
                (((double)(contador))/((double)totallinhas) * 100, 0);
            var barra = tools.BarraProgressoTexto
                    ('#', 12, (int) (percentagem));
            System.Console.WriteLine($"{barra} - " + 
                $"{(int) (percentagem)}% - {contador} de {totallinhas}");
        }

        static void TesteDoProgressBar() {
		Console.Write("Preparando o ambiente...");
		using (var progress = new ConsoleProgressBar()) {
			for (int i = 0; i <= 100; i++) {
				progress.Report((double) i / 100);
				Thread.Sleep(10);
			}
		}
		Console.WriteLine("Done.");
	}
    }
}
