using System;
using System.IO;
using System.Threading;

namespace LibFileTools
{
    public class ReadingCSV
    {
        public delegate void LinhaDelegate(string [] colsLinha, long linha, long total);
        public static void LerArquivoCsv(string pathFileName, LinhaDelegate process)
        {
            try {
                long tamanho = 0;
                try {
                    tamanho = LinhasArquivoCsv(pathFileName);
                } catch (Exception exFile01) {
                    Thread.Sleep(100);
                    try {
                        tamanho = LinhasArquivoCsv(pathFileName);
                    } catch (Exception exFile02) {
                        Thread.Sleep(100);
                        try {
                            tamanho = LinhasArquivoCsv(pathFileName);
                        } catch (Exception exFile03) {
                            Thread.Sleep(100);
                            tamanho = LinhasArquivoCsv(pathFileName);
                        }
                    }
                }
                StreamReader csvReader = null;
                try {
                    csvReader = new StreamReader(pathFileName);
                } catch (Exception exFile01) {
                    Thread.Sleep(100);
                    try {
                        csvReader = new StreamReader(pathFileName);
                    } catch (Exception exFile02) {
                        Thread.Sleep(100);
                        try {
                            csvReader = new StreamReader(pathFileName);
                        } catch (Exception exFile03) {
                            Thread.Sleep(100);
                            csvReader = new StreamReader(pathFileName);
                        }
                    }
                }
                int totalLinhasProcessadas = 0;
                while (!csvReader.EndOfStream) {

                    var linha = csvReader.ReadLine();
                    var valores = linha.Split(',');
                    totalLinhasProcessadas++;
                    // o que você precisa fazer aqui
                    process(valores, totalLinhasProcessadas, tamanho);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(
                    $"Erro no processamento do arquivo {pathFileName} : {ex.Message} --> {ex.StackTrace}");
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