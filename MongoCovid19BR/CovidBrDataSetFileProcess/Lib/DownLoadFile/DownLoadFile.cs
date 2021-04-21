using System;
using System.Net;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using CovidBrDataSetFileProcess.Lib.ProgressBar;

namespace CovidBrDataSetFileProcess.Lib.DownLoadFile
{
    public class DownLoadFile
    {
        private ConsoleProgressBar progress;
        private string currentText = string.Empty;

        public void DownLoadFileInBackgroundByProgBar4(string address)
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
                // client.DownloadDataCompleted += DownloadDataCompletedCallback4;
                // Specify a progress notification handler.
                client.DownloadProgressChanged += DownloadProgressCallbackByProgBar4;

                using (ConsoleProgressBar progress = new ConsoleProgressBar())
                {
                    client.DownloadFileAsync(uri, pathString);
                }
            }
            System.Console.WriteLine("Aguarde conclusão do Download...");
            Console.ReadLine();
        }

        private void DownloadProgressCallbackByProgBar4(object sender, DownloadProgressChangedEventArgs e)
        {
            if(this.progress != null)
                this.progress.Report(e.ProgressPercentage);

            
            UpdateText($"{e.ProgressPercentage} % Completo... " +
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
            Console.ReadLine();
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