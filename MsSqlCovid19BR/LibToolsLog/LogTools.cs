using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LibToolsLog
{
    public class LogTools
    {
        
        public static async void LogErroToFile( string mensagem, string dadosDoErro)
        {
            string diretorioLog = ConfigurationManager.AppSettings["dirlog"].Replace('/', Path.DirectorySeparatorChar);
            // Criar Diretório se não existe
            if(!System.IO.Directory.Exists(diretorioLog))
                System.IO.Directory.CreateDirectory(diretorioLog);
                
            string FileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm")
                    .Replace("/","-").Replace(":","_") +
                    " - Registro de Erros.log";
            string fileErroLog = System.IO.Path.Combine(diretorioLog, FileName);

            string textoCsv = $"ERRO at {DateTime.Now.ToString()}: {mensagem} --> {dadosDoErro}";
            try {
                await WriterLogFile(fileErroLog, textoCsv);
            } catch (Exception ex) {
                Thread.Sleep(100);
                try {
                    await WriterLogFile(fileErroLog, textoCsv);
                } catch (Exception ex2) {
                    Thread.Sleep(100);
                    try {
                        await WriterLogFile(fileErroLog, textoCsv);
                    } catch (Exception ex3) {
                    }
                }
            }
        }

        private static async Task WriterLogFile(string fileErroLog, string textoCsv) {
            if (!File.Exists(fileErroLog)) {
                using (var stream = new StreamWriter(fileErroLog)) {
                    await stream.WriteLineAsync(textoCsv);
                }
            } else {
                using (StreamWriter sw = File.AppendText(fileErroLog)) {
                    await sw.WriteLineAsync(textoCsv);
                }
            }
        }
    }
}
