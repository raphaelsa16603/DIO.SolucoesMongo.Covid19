using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibConsoleProgressBar;

namespace LibFileDownload
{
    public class DownLoadFile
    {
        private ConsoleProgressBar progress = null;
        private string currentText = string.Empty;
        private int _percentagem = 0;

        public void DownLoadFileInBackgroundByProgBar4(string address) {
            string diretorioDataSet = ConfigurationManager.AppSettings["dir"].Replace('/', Path.DirectorySeparatorChar);
            string arquivoDB = ConfigurationManager.AppSettings["file"].Replace('/', Path.DirectorySeparatorChar);
            string pathString = System.IO.Path.Combine(diretorioDataSet, arquivoDB);
            System.Console.WriteLine($"Arquivo : {pathString}");
            System.Console.WriteLine("");
            // Criar Diretório se não existe
            if (!System.IO.Directory.Exists(diretorioDataSet))
                System.IO.Directory.CreateDirectory(diretorioDataSet);

            //WebClient client = new WebClient();
            using (WebClient client = new WebClient()) {
                Uri uri = new Uri(address);
                // Problema para baixar o arquivo ??? De uma hora para outra
                //  deixou de baixar o arquivo .gz do site pelo programa.
                client.Headers["Cookie"] = "security=true";
                client.Encoding = System.Text.Encoding.UTF8;
                client.Headers.Add("user-agent",
                "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");

                // Specify a DownloadFileCompleted handler here...
                // client.DownloadDataCompleted += DownloadDataCompletedCallback4;
                // Specify a progress notification handler.
                client.DownloadProgressChanged += DownloadProgressCallbackByProgBar4;

                using (ConsoleProgressBar progress = new ConsoleProgressBar()) {
                    try {
                        client.DownloadFileAsync(uri, pathString);
                    } catch (System.Exception ex) {
                        System.Console.WriteLine($"Erro no Dowload {ex.Message}!");
                        client.DownloadFile(uri, pathString);
                    }
                }
            }
            VerificarArquivoDownloadNormal(address, pathString);

        }

        private void VerificarArquivoDownloadNormal(string address, string pathString) {
            Thread.Sleep(100);
            // Tentativas extras de fazer o donwload sincrono
            FileInfo fi = null;

            for (int i = 0; i < 15; i++) {
                fi = new FileInfo(pathString);
                if (fi == null) {
                    Thread.Sleep(100);
                } else if (fi.Length <= 0) {
                    Thread.Sleep(100);
                }
            }

            if (fi == null) {
                using (WebClient client = new WebClient()) {
                    Uri uri = new Uri(address);
                    client.DownloadFile(uri, pathString);
                }
            } else if (fi.Length <= 0) {
                using (WebClient client = new WebClient()) {
                    Uri uri = new Uri(address);
                    client.DownloadFile(uri, pathString);
                }
            }
            System.Console.WriteLine("Aguarde conclusão do Download...");

            for(int esperar = 0; this._percentagem < 100; esperar++) {
                Thread.Sleep(100);
            }
        }

        private void DownloadProgressCallbackByProgBar4(object sender, DownloadProgressChangedEventArgs e)
        {
            if (this.progress != null) {
                this.progress.Report(e.ProgressPercentage);
            }

            this._percentagem = e.ProgressPercentage;
            string barra = BarraProgressoTexto('#', 12, e.ProgressPercentage);
            UpdateText($"{barra} - {e.ProgressPercentage} % Completo... " +
            $" Baixado {e.BytesReceived} de {e.TotalBytesToReceive} bytes...");
            
        }

        private void UpdateText(string text) 
        {
            // Get length of common portion
            int commonPrefixLength = 0;
            int commonLength = Math.Min(currentText.Length, text.Length);
            while (commonPrefixLength < commonLength && text[commonPrefixLength] == currentText[commonPrefixLength]) {
                commonPrefixLength++;
            }

            // Backtrack to the first differing character
            StringBuilder outputBuilder = new StringBuilder();
            outputBuilder.Append('\b', currentText.Length - commonPrefixLength);

            // Output new suffix
            outputBuilder.Append(text.Substring(commonPrefixLength));

            // If the new text is shorter than the old one: delete overlapping characters
            int overlapCount = currentText.Length - text.Length;
            if (overlapCount > 0) {
                outputBuilder.Append(' ', overlapCount);
                outputBuilder.Append('\b', overlapCount);
            }

            Console.Write(outputBuilder);
            currentText = text;
	    }

        private string BarraProgressoTexto(char simbolo, int tamanho, double percentagem) 
        {
            int progress = 0;

            if(percentagem > 0)
            {
                progress = (int) Math.Round(
                        (tamanho * (percentagem/100)),0);
            }
            else if (percentagem == 0 )
                progress = 0;
            else if (percentagem < 0 )
                progress = 0;
            else if (percentagem > 100 )
                progress = 0;

			int percent = (int) (percentagem * 100);
            string text = string.Format("[{0}{1}]",
				new string('#', progress), 
                new string('-', Math.Abs(tamanho - progress)));

            return text;
        }

        public static async void ToDo(string address)
        {
            await Task.Run(() => DownLoadFileInBackground4(address));
        }

        public static void DownLoadFileInBackground4(string address)
        {
            string diretorioDataSet = ConfigurationManager.AppSettings["dir"];
            string arquivoDB = ConfigurationManager.AppSettings["file"];
            string pathString = System.IO.Path.Combine(diretorioDataSet, arquivoDB);
            System.Console.WriteLine($"Arquivo : {pathString}");
            System.Console.WriteLine("");
            // Criar Diretório se não existe
            if(!System.IO.Directory.Exists(diretorioDataSet))
                System.IO.Directory.CreateDirectory(diretorioDataSet);

            //WebClient client = new WebClient();
            using (WebClient client = new WebClient())
            {
                Uri uri = new Uri(address);

                // Specify a DownloadFileCompleted handler here...
                client.DownloadDataCompleted += DownloadDataCompletedCallback4;
                // Specify a progress notification handler.
                client.DownloadProgressChanged += DownloadProgressCallback4;

                client.DownloadFileAsync(uri, pathString);
            }
            
        }

        private static void DownloadProgressCallback4(object sender, DownloadProgressChangedEventArgs e)
        {
            // progress.Report(e.ProgressPercentage);
            // Displays the operation identifier, and the transfer progress.
            Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...",
                (string)e.UserState,
                e.BytesReceived,
                e.TotalBytesToReceive,
                e.ProgressPercentage);
        }

        private static void DownloadDataCompletedCallback4(object sender, DownloadDataCompletedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.
            Console.WriteLine("Completo ... {0} 100% complete...",
                (string)e.UserState);
        }
    }
}
