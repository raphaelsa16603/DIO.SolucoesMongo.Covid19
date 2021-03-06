using System;
using System.IO;
using System.IO.Compression;

namespace DownloadFile.Lib.FileTools
{
    public class CompactacaoArquivo
    {
        
            public static void Compress(string directoryPath)
            {
                DirectoryInfo directorySelected = new DirectoryInfo(directoryPath);
                foreach (FileInfo fileToCompress in directorySelected.GetFiles())
                {
                    using (FileStream originalFileStream = fileToCompress.OpenRead())
                    {
                        if ((File.GetAttributes(fileToCompress.FullName) &
                        FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                        {
                            using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                            {
                                using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                                CompressionMode.Compress))
                                {
                                    originalFileStream.CopyTo(compressionStream);
                                }
                            }
                            FileInfo info = new FileInfo(
                                directoryPath + Path.DirectorySeparatorChar + 
                                fileToCompress.Name + ".gz");
                            Console.WriteLine
                            ($"Compressed {fileToCompress.Name} from {fileToCompress.Length.ToString()} to {info.Length.ToString()} bytes.");
                        }
                    }
                }
            }

            public static void Decompress(string pathFileGz)
            {
                FileInfo fileToDecompress = new FileInfo(pathFileGz);
                long tamanho = fileToDecompress.Length;
                if(tamanho > 0)
                {
                    using (FileStream originalFileStream = fileToDecompress.OpenRead())
                    {
                        string currentFileName = fileToDecompress.FullName;
                        string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                        using (FileStream decompressedFileStream = File.Create(newFileName))
                        {
                            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                            {
                                decompressionStream.CopyTo(decompressedFileStream);
                                Console.WriteLine($"Decompressed: {fileToDecompress.Name}");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"N??o foi poss??vel descompactar o arquivo: {fileToDecompress.Name}");
                }
            }
    }
}