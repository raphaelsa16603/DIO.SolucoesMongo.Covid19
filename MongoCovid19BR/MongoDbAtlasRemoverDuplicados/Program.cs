using System;
using System.Configuration;
using System.IO;
using System.Threading;
using MongoDbAtlasRemoverDuplicados.Business;
using LibConsoleProgressBar;
using LibFileDownload;
using LibFileTools;
using LibWeb;

namespace MongoDbAtlasRemoverDuplicados
{
    class Program
    {
        static void Main(string[] args)
        {
            string diretorioDataSet = ConfigurationManager.AppSettings["dir"];
            // Criar Diretório se não existe
            if(!System.IO.Directory.Exists(diretorioDataSet))
                System.IO.Directory.CreateDirectory(diretorioDataSet);
            string arquivoDB = ConfigurationManager.AppSettings["file"];
            string pathString = System.IO.Path.Combine(diretorioDataSet, arquivoDB);
            DirectoryInfo directoryData = new DirectoryInfo(diretorioDataSet);

            // TODO: Ler a página https://brasil.io/dataset/covid19/files/ 
            // e pegar a data do arquivo -- Classe Ler dados da página!
            string Data = "";
            try
            {
                Data = LoadInfoUrl.GetDataUrlCovid19BrFiles("","");
            }
            catch (System.Exception)
            {
                bool existeArquivoCsv = directoryData.GetFiles("*.csv").Length > 0;
                foreach (FileInfo fileToCsv in directoryData.GetFiles("*.csv"))
                {
                    Data = fileToCsv.Name.Replace("/","-").Replace(" - caso_full.csv", "").Trim();
                }
            }
            System.Console.WriteLine($"Data do arquivo {Data}");
            
            // TODO: -- Classe para fazer barra de progresso na tela do console
            TesteDoProgressBar();

            bool existeArquivoCsvDoDia = false;
            foreach (FileInfo fileToCsv in directoryData.GetFiles("*.csv"))
            {
                existeArquivoCsvDoDia = 
                    fileToCsv.FullName.Contains(Data.Trim().Replace("/","-"));
            }
            if(!existeArquivoCsvDoDia)
            {
                // Se arquivo do dia não existe então remove todos os outros *.csv da pasta
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
            
            // --- Registrar os dados do CSV no banco de dados SQLite é muito lento
            // --- Solução é enviar os dados diretamente para a API RestFull

            // -- Devido ao problemas com o download do arquivo a parte de leitura
            // -- do arquivo csv a atualização no Banco de Dados será feito separadamente

            // TODO: Ler o arquivo csv linha por linha e colocar no banco de dados local
            // Banco de dados SQLite!
            // -- Classe para ler arquivo texto ou csv
            
            DirectoryInfo directoryFilesCsv = new DirectoryInfo(diretorioDataSet);
            // -- Classe para ler dados do arquivo csv, 
            // pegando o cabeçalho e lendo coluna por coluna e linha por linha
            // -- Classe do EntityFramework para salvar os dados lidos e colocar na base de dados
            // -- Classe e Domain / Controller para verificar registro por registro do csv 
            // para ver se os dados do arquivo já não estão armazenados no banco de dados, 
            // se não tiver, insere o registo com a fleg novo e o uId gerado, 
            // e se tiver, verificar se há atualização e atualiza o registro no BD, sentando a flag
            // atualizado, mantendo o uId original. 
            // var ForDb = new RegistroDeDadosDbLocal(new Data.MongoDB());

            /* --- Limpar o arquivo CSV para importar pelo MongoDB Compass não funciona
            // --- Importação limitada a 8.000 registros ....
            System.Console.WriteLine("----------------------------------------------------");
            System.Console.WriteLine("Limpando Arquivo CVS com dados válidos...");
            System.Console.WriteLine("----------------------------------------------------");
            // Processando os arquivos csv e colocando no Banco de Dados SQLite
            foreach (FileInfo fileToCsv in directoryFilesCsv.GetFiles("*.csv"))
            {
                ReadingCSV.LerArquivoCsv(fileToCsv.FullName, ForDb.processarArqCsvInserirNovoArquivoCsvLimpo);
            }
            System.Console.WriteLine("\n");
            */
            string uf = "PB";
            try
            {
                uf = ConfigurationManager.AppSettings["incrementalUF"];
            }
            catch (System.Exception)
            {
                uf = "PB";
            }
            string data = "";
            try
            {
                data = ConfigurationManager.AppSettings["incrementalData"];
                var teste = DateTime.Parse(data);
            }
            catch (System.Exception)
            {
                data = "2021-01-13";
            }
            using (var ForDb = new RegistroDeDadosDbLocal(new Data.MongoDB(), uf, data))
            {
                
                System.Console.WriteLine("-------------------------------------------------------");
                System.Console.WriteLine("Atualizando Banco de Dados ModoDB Atlas com novos dados");
                System.Console.WriteLine("-------------------------------------------------------");
                System.Console.WriteLine($"---   Filtro:            UF == {uf}  -------");
                System.Console.WriteLine($"---   Filtro:            Data >= {data}  -------");
                System.Console.WriteLine("------------------------------------------------------");
                // Processando os arquivos csv e colocando no Banco de Dados MongoDB
                foreach (FileInfo fileToCsv in directoryFilesCsv.GetFiles("*.csv"))
                {
                    ReadingCSV.LerArquivoCsv(fileToCsv.FullName, ForDb.processarArqCsvInserirNoDB);
                }
                System.Console.WriteLine("\n");
            }

            //Atualiza arquivo de configuração 
            var novaDataConfig = DateTime.Parse(Data).AddDays(-2);
            AddUpdateAppSettings("incrementalData", novaDataConfig.ToString("yyyy-MM-dd"));
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


        private static void AddUpdateAppSettings(string key, string value)  
        {  
            try  
            {  
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);  
                var settings = configFile.AppSettings.Settings;  
                if (settings[key] == null)  
                {  
                    settings.Add(key, value);  
                }  
                else  
                {  
                    settings[key].Value = value;  
                }  
                configFile.Save(ConfigurationSaveMode.Modified);  
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);  
            }  
            catch (ConfigurationErrorsException)  
            {  
                Console.WriteLine("Error writing app settings");  
            }  
        } 
    }
}
