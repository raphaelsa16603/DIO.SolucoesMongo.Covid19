using System.IO;
using System.Collections.Generic;
using System;

namespace CovidBrDataSetFileProcess.Lib.FileTools
{
    public class ReadingCSV
    {
        public delegate void LinhaDelegate(string [] colsLinha);
        public static void LerArquivoCsv(string pathFileName, LinhaDelegate process)
        {
            StreamReader csvReader = new StreamReader(pathFileName);

            while (!csvReader.EndOfStream)
            {
                var linha = csvReader.ReadLine();
                var valores = linha.Split(',');

                // o que você precisa fazer aqui
                process(valores);
            }
        }
    }
}