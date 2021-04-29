using System.IO;
using System.Collections.Generic;
using System;

namespace DownloadFile.Lib.FileTools
{
    public class ReadingCSV
    {
        public delegate void LinhaDelegate(string [] colsLinha, long linha, long total);
        public static void LerArquivoCsv(string pathFileName, LinhaDelegate process)
        {
            long tamanho = LinhasArquivoCsv(pathFileName);
            StreamReader csvReader = new StreamReader(pathFileName);
            int totalLinhas = 0;
            while (!csvReader.EndOfStream)
            {
                
                var linha = csvReader.ReadLine();
                var valores = linha.Split(',');
                totalLinhas++;
                // o que você precisa fazer aqui
                process(valores, totalLinhas, tamanho);
            }
        }

        public static long LinhasArquivoCsv(string pathFileName)
        {
            long count = 0;
            using (StreamReader r = new StreamReader(pathFileName))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    count++;
                }
            }

            return count;
        }
    }
}