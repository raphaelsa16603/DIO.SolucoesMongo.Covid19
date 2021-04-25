using System;
using System.Configuration;
using System.IO;

namespace CovidBrDataSetFileProcess.Lib.ToolsLogs
{
    public class LogTools
    {
        
        public static async void LogErroToFile( string mensagem, string dadosDoErro)
        {
            string diretorioLog = ConfigurationManager.AppSettings["dirlog"];
            // Criar Diretório se não existe
            if(!System.IO.Directory.Exists(diretorioLog))
                System.IO.Directory.CreateDirectory(diretorioLog);
                
            string FileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm")
                    .Replace("/","-").Replace(":","_") +
                    " - Registro de Erros.log";
            string fileErroLog = System.IO.Path.Combine(diretorioLog, FileName);

            string textoCsv = $"ERRO at {DateTime.Now.ToString()}: {mensagem} --> {dadosDoErro}";
            if (!File.Exists(fileErroLog))
            {
                using (var stream = new StreamWriter(fileErroLog))
                {
                    await stream.WriteLineAsync(textoCsv);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(fileErroLog))
                {
                    await sw.WriteLineAsync(textoCsv);
                }
            }
        }
    }
}