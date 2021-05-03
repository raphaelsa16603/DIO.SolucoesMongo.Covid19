using System;
using System.Configuration;
using System.IO;
using IncrementalUfFileProcessSQLite.Controller;
using IncrementalUfFileProcessSQLite.Model;
using LibConsoleProgressBar;
using LibToolsLog;

namespace IncrementalUfFileProcessSQLite.Business
{
    public class RegistroDeDadosDbLocal : IDisposable
    {
        ToolsProgressBar tools = new ToolsProgressBar();
        string fileErroCsv = "";
        string fileErroCsvLimpeza = "";
        string fileCsvLimpo = "";
        string _UfFilter = "PB";

        DadosCovidController controller;

        public RegistroDeDadosDbLocal(string incrementalData)
        {
            if(incrementalData.Trim().Equals(""))
            {
                string Uf = "";
                try
                {
                    Uf = ConfigurationManager.AppSettings["incrementalUF"];
                    var teste = DateTime.Parse(Uf);
                }
                catch (System.Exception)
                {
                    Uf = "PB";
                }
                
                if (!Uf.Trim().Equals(""))
                {
                    _UfFilter = Uf;
                }
            }
            string diretorioDataErro = ConfigurationManager.AppSettings["dirCsvErro"];
            // Criar Diretório se não existe
            if(!System.IO.Directory.Exists(diretorioDataErro))
                System.IO.Directory.CreateDirectory(diretorioDataErro);

            string FileName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm")
                    .Replace("/","-").Replace(":","_") +
                    " - Import DataSet ERRO.csv";
            fileErroCsv = System.IO.Path.Combine(diretorioDataErro, FileName);

            string FileNameLimpeza = DateTime.Now.ToString("yyyy-MM-dd_HH-mm")
                    .Replace("/","-").Replace(":","_") +
                    " - Import DataSet ERRO na Limpeza.csv";
            fileErroCsvLimpeza = System.IO.Path.Combine(diretorioDataErro, FileNameLimpeza);

            string diretorioDataSetLimpo = ConfigurationManager.AppSettings["dirCsvLimpo"];
            // Criar Diretório se não existe
            if(!System.IO.Directory.Exists(diretorioDataSetLimpo))
                System.IO.Directory.CreateDirectory(diretorioDataSetLimpo);

            string FileNameLimpo = DateTime.Now.ToString("yyyy-MM-dd_HH-mm")
                    .Replace("/","-").Replace(":","_") +
                    " - DataSet Tratado e Limpo.csv";
            
            fileCsvLimpo = System.IO.Path.Combine(diretorioDataSetLimpo, FileNameLimpo);

            controller = DadosCovidController.GetInstance(new Context());
        }
        
        public void processarArqCsvInserirNoDB( string [] listaCampos, long contador, long totallinhas)  
        {
            DadosCovid oDado = null;
            if(contador > 1)
            {
                try
                {

                    // Preencher Classe de Dados
                    oDado = new DadosCovid();
                    ExtrairDadosDoCvsParaDb(listaCampos, oDado);

                    //System.Console.WriteLine(oDado.ToString());

                }
                catch (System.Exception ex)
                {
                    // Registrar no arquivo de log ...
                    LogTools.LogErroToFile
                        ($"Erro ao registrar dados no DB: {ex.Message}", ex.StackTrace);
                    LogTools.LogErroToFile("________________________________________", "");
                    LogTools.LogErroToFile($"ERRO IMPORTAÇÃO: {ex.Message}", "");
                    string registro = "";
                    foreach (string campo in listaCampos)
                    {
                        registro += $"{campo},";
                    }
                    LogTools.LogErroToFile(registro, "...");
                    LogTools.LogErroToFile("________________________________________","");
                    // Registrar dados não incluido no DB
                    ErroToFileDataErro(listaCampos);
                }

                if(oDado != null)
                {
                    int codigo = 0;
                    if(oDado.state != null)
                    {
                        if(oDado.state.Trim().ToUpper().Equals(_UfFilter.Trim().ToUpper()))
                        {
                            try
                            {
                                // Conversão para metódos sincronos funcionou mais deixou super lento
                                DadosCovid DbObj = controller.Pesquisa(oDado);
                                if (DbObj == null)
                                    if(contador < totallinhas)
                                        codigo = controller.Cadastro(oDado);
                                    else
                                        codigo = controller.CadastroSimples(oDado);
                            }
                            catch (System.Exception ex)
                            {
                                // Se der erro é para registrar em arquivo de log
                                LogTools.LogErroToFile($" Erro no cadastro {ex.Message}", ex.StackTrace);
                            }
                        }
                        else
                        {
                            // Registro dispensado
                        }
                    }
                }

                int percentagem = (int)Math.Round
                        (((double)(contador)) / ((double)totallinhas) * 100, 0);
                var barra = tools.BarraProgressoTexto
                        ('#', 30, (int)(percentagem));
                // System.Console.WriteLine($"{barra} - " +
                //     $"{(int)(percentagem)}% - {contador} de {totallinhas}");
                tools.UpdateText($"{barra} - " + 
                    $"{(int) (percentagem)}% - {contador} de {totallinhas}");
            }
        }

        private void ExtrairDadosDoCvsParaDb(string[] listaCampos, DadosCovid oDado)
        {
            // public int Id { get; set; }
            // public string city { get; set; }

            try
            {
                oDado.city = listaCampos[0].Trim();
            }
            catch (System.Exception)
            {
                oDado.city = "";
            }

            // Campos principais --------------------------------------------
            // public string city_ibge_code { get; set; }
            oDado.city_ibge_code = listaCampos[1].Trim();
            if (listaCampos[1].Trim().Equals(""))
                throw new Exception("Sem o Código [city_ibge_code]");
            // public DateTime date { get; set; }
            oDado.date = Convert.ToDateTime(listaCampos[2].Trim());
            // Campos principais --------------------------------------------

            // public string epidemiological_week { get; set; }
            try
            {
                oDado.epidemiological_week = listaCampos[3].Trim();
            }
            catch (System.Exception)
            {
                oDado.epidemiological_week = "";
            }

            // public long estimated_population { get; set; }
            try
            {
                oDado.estimated_population = (long)Convert.ToDecimal(listaCampos[4].Trim());
            }
            catch (System.Exception)
            {
                oDado.estimated_population = 0;
            }
            // public long estimated_population_2019 { get; set; }
            try
            {
                oDado.estimated_population_2019 = (long)Convert.ToDecimal(listaCampos[5].Trim());
            }
            catch (System.Exception)
            {
                oDado.estimated_population_2019 = 0;
            }
            // public string is_last { get; set; }
            try
            {
                oDado.is_last = listaCampos[6].Trim();
            }
            catch (System.Exception)
            {
                oDado.is_last = "";
            }
            // public string is_repeated { get; set; }
            try
            {
                oDado.is_repeated = listaCampos[7].Trim();
            }
            catch (System.Exception)
            {
                oDado.is_repeated = "";
            }
            // public long city_ibglast_available_confirmede_code { get; set; }
            oDado.city_ibglast_available_confirmede_code =
                (long)Convert.ToDecimal(listaCampos[8].Trim());
            // public double last_available_confirmed_per_100k_inhabitants { get; set; }
            // oDado.last_available_confirmed_per_100k_inhabitants = 
            //     Double.Parse(listaCampos[9].Trim());
            try
            {
                oDado.last_available_confirmed_per_100k_inhabitants =
                Double.Parse(listaCampos[9].Trim());
            }
            catch (System.Exception)
            {
                oDado.last_available_confirmed_per_100k_inhabitants = 0;
            }
            // public DateTime last_available_date { get; set; }
            oDado.last_available_date = Convert.ToDateTime(listaCampos[10].Trim());
            // public double last_available_death_rate { get; set; }
            try
            {
                oDado.last_available_death_rate = Double.Parse(listaCampos[11].Trim());
            }
            catch (System.Exception)
            {
                oDado.last_available_death_rate = 0;
            }
            // public long last_available_deaths { get; set; }
            try
            {
                oDado.last_available_deaths = (long)Convert.ToDecimal(listaCampos[12].Trim());
            }
            catch (System.Exception)
            {
                oDado.last_available_deaths = 0;
            }
            // public long order_for_place { get; set; }
            oDado.order_for_place = (long)Convert.ToDecimal(listaCampos[13].Trim());
            // public string place_type { get; set; }
            oDado.place_type = listaCampos[14].Trim();
            // public string state { get; set; }
            oDado.state = listaCampos[15].Trim();
            // public long new_confirmed { get; set; }
            oDado.new_confirmed = (long)Convert.ToDecimal(listaCampos[16].Trim());
            // public long new_deaths { get; set; }
            oDado.new_deaths = (long)Convert.ToDecimal(listaCampos[17].Trim());
            // public string uId {get; set;}
            oDado.uId = this.GerarUId();
            // public bool dadosNovos {get; set;}
            oDado.dadosNovos = true;
            // public bool dadosAtualizados {get; set;}
            oDado.dadosAtualizados = false;
        }

        private async void ErroToFileDataErro( string [] listaCampos )
        {
            string textoCsv = "";
            foreach (string campo in listaCampos)
            {
                textoCsv += $"{campo},";
            }
            if (!File.Exists(fileErroCsv))
            {
                using (var stream = new StreamWriter(fileErroCsv))
                {
                    await stream.WriteLineAsync(textoCsv);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(fileErroCsv))
                {
                    await sw.WriteLineAsync(textoCsv);
                }
            }
        }

        public string GerarUId()
        { 
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();

            return myuuidAsString;
        }

        public int GerarId()
        { 
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            string numerosUid = "";
            // Pegar só os números
            foreach(char l in myuuidAsString)
            {
                if(l >= '0' && l <= '9')
                    numerosUid += l;
            }

            int iRet = 0;
            try
            {
                iRet = int.Parse(numerosUid);    
            }
            catch (Exception)
            {
                iRet = 1;
            }
            
            return iRet;
        }


        public void processarArqCsvInserirNovoArquivoCsvLimpo( string [] listaCampos, long contador, long totallinhas)  
        {
            DadosCovid oDado = null;
            if(contador > 1)
            {
                try
                {

                    // Preencher Classe de Dados
                    oDado = new DadosCovid();
                    ExtrairDadosDoCvsParaDb(listaCampos, oDado);

                    //System.Console.WriteLine(oDado.ToString());

                }
                catch (System.Exception ex)
                {
                    // Registrar no arquivo de log ...
                    LogTools.LogErroToFile
                        ($"Erro ao registrar dados no DB: {ex.Message}", ex.StackTrace);
                    LogTools.LogErroToFile("________________________________________", "");
                    LogTools.LogErroToFile($"ERRO IMPORTAÇÃO: {ex.Message}", "");
                    string registro = "";
                    foreach (string campo in listaCampos)
                    {
                        registro += $"{campo},";
                    }
                    LogTools.LogErroToFile(registro, "...");
                    LogTools.LogErroToFile("________________________________________","");
                    // Registrar dados não incluido no DB
                    ErroToFileDataErroLimpeza(listaCampos);
                }
                

                if(oDado != null)
                {
                    try
                    {
                        SalvarCamposNovoArquivoCsvLimpo(listaCampos);
                    }
                    catch (System.Exception ex)
                    {
                        // Se der erro é para registrar em arquivo de log
                        LogTools.LogErroToFile($" Erro no cadastro {ex.Message}", ex.StackTrace);
                    }
                }
            }
            else 
            {
                SalvarCamposNovoArquivoCsvLimpo(listaCampos);
            }
            int percentagem = (int)Math.Round
                    (((double)(contador)) / ((double)totallinhas) * 100, 0);
            var barra = tools.BarraProgressoTexto
                    ('#', 30, (int)(percentagem));
            // System.Console.WriteLine($"{barra} - " +
            //     $"{(int)(percentagem)}% - {contador} de {totallinhas}");
            tools.UpdateText($"{barra} - " + 
                $"{(int) (percentagem)}% - {contador} de {totallinhas}");

        }

        private async void SalvarCamposNovoArquivoCsvLimpo( string [] listaCampos )
        {
            string textoCsv = "";
            foreach (string campo in listaCampos)
            {
                textoCsv += $"{campo},";
            }
            if (!File.Exists(fileCsvLimpo))
            {
                using (var stream = new StreamWriter(fileCsvLimpo))
                {
                    await stream.WriteLineAsync(textoCsv);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(fileCsvLimpo))
                {
                    await sw.WriteLineAsync(textoCsv);
                }
            }
        }

        private async void ErroToFileDataErroLimpeza( string [] listaCampos )
        {
            string textoCsv = "";
            foreach (string campo in listaCampos)
            {
                textoCsv += $"{campo},";
            }
            if (!File.Exists(fileErroCsvLimpeza))
            {
                using (var stream = new StreamWriter(fileErroCsvLimpeza))
                {
                    await stream.WriteLineAsync(textoCsv);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(fileErroCsvLimpeza))
                {
                    await sw.WriteLineAsync(textoCsv);
                }
            }
        }

        public void Dispose()
        {
            this.controller.Dispose();
        }
    }
}