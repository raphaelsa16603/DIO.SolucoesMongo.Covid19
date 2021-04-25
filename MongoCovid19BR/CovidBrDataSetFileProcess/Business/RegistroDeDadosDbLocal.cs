using System;
using System.Configuration;
using System.IO;
using CovidBrDataSetFileProcess.Controller;
using CovidBrDataSetFileProcess.Lib.ProgressBar;
using CovidBrDataSetFileProcess.Model;
using CovidBrDataSetFileProcess.Lib.ToolsLogs;

namespace CovidBrDataSetFileProcess.Business
{
    public class RegistroDeDadosDbLocal
    {
        ToolsProgressBar tools = new ToolsProgressBar();
        string fileErroCsv = "";

        DadosCovidController controller;

        public RegistroDeDadosDbLocal()
        {
            string diretorioDataSet = ConfigurationManager.AppSettings["dir"];
            string FileName = DateTime.Now.ToString().Replace("/","-").Replace(":","_") +
                    " - Import DataSet ERRO.csv";
            fileErroCsv = System.IO.Path.Combine(diretorioDataSet, FileName);

            controller = new DadosCovidController(new Context());
        }
        
        public async void processarArqCsvInserirNoDB( string [] listaCampos, long contador, long totallinhas)  
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
                    try
                    {
                        DadosCovid DbObj = await controller.Pesquisa(oDado);
                        if (DbObj == null)
                            controller.Cadastro(oDado);
                    
                    }
                    catch (System.Exception ex)
                    {
                        // Se der erro é para registrar em arquivo de log
                        LogTools.LogErroToFile($" Erro no cadastro {ex.Message}", ex.StackTrace);
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

        private void ErroToFileDataErro( string [] listaCampos )
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
                    stream.WriteLine(textoCsv);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(fileErroCsv))
                {
                    sw.WriteLine(textoCsv);
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

    }
}