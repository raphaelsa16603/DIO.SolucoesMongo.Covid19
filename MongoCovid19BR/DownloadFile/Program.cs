using System;
using System.Configuration;
using System.IO;
using DownloadFile.Lib.DownLoadFile;
using DownloadFile.Lib.FileTools;
using DownloadFile.Lib.Web;

namespace DownloadFile
{
    class Program
    {
        static void Main(string[] args)
        {
            string Data = LoadInfoUrl.GetDataUrlCovid19BrFiles();
            System.Console.WriteLine($"Data do arquivo {Data}");
            
            string diretorioDataSet = ConfigurationManager.AppSettings["dir"];
            // Criar Diretório se não existe
            if(!System.IO.Directory.Exists(diretorioDataSet))
                System.IO.Directory.CreateDirectory(diretorioDataSet);
            string arquivoDB = ConfigurationManager.AppSettings["file"];
            string pathString = System.IO.Path.Combine(diretorioDataSet, arquivoDB);

            DirectoryInfo directoryData = new DirectoryInfo(diretorioDataSet);
            bool existeArquivoCsvDoDia = false;
            foreach (FileInfo fileToCsv in directoryData.GetFiles("*.csv"))
            {
                existeArquivoCsvDoDia = 
                    fileToCsv.FullName.Contains(Data.Trim().Replace("/","-"));
            }

            directoryData = new DirectoryInfo(diretorioDataSet);
            foreach (FileInfo fileToCsv in directoryData.GetFiles("*.csv"))
            {
                fileToCsv.Delete();
            }
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
    }
}
