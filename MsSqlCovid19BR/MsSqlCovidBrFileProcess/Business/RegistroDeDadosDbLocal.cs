using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LibConsoleProgressBar;
using LibToolsLog;
using MsSqlCovidBrFileProcess.Controller;
using MsSqlCovidBrFileProcess.Model;

namespace MsSqlCovidBrFileProcess.Business
{
    public class RegistroDeDadosDbLocal : IDisposable
    {
        ToolsProgressBar tools = new ToolsProgressBar();
        string fileErroCsv = "";
        string fileErroCsvLimpeza = "";
        string fileCsvLimpo = "";

        DadosCovidController controller;

        public RegistroDeDadosDbLocal()
        {
            string diretorioDataErro = ConfigurationManager.AppSettings["dirCsvErro"].Replace('/', Path.DirectorySeparatorChar);
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

            string diretorioDataSetLimpo = ConfigurationManager.AppSettings["dirCsvLimpo"].Replace('/', Path.DirectorySeparatorChar);
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
                    try
                    {
                        // Conversão para metódos sincronos funcionou mais deixou super lento
                        DadosCovid DbObj = controller.Pesquisa(oDado);
                        if (DbObj == null) {
                            if (contador < totallinhas) // TODO: Revisar essa lógica
                                codigo = controller.Cadastro(oDado);
                            else
                                codigo = controller.CadastroSimples(oDado);
                        } else {
                            // Compara dados do arquivo com dados da tabela
                            if (!OsDadosSaoIguais(oDado, DbObj)) {
                                // Verificação extra
                                // Comparar date e city_ibge_code
                                if (oDado.city_ibge_code.Trim() == DbObj.city_ibge_code.Trim()) {
                                    if (oDado.date.CompareTo(DbObj.date) == 0) {
                                        //Não sendo iguais os outros dados atualiza o registro do DB
                                        DadosCovid DbAtualizado = controller.Update(DbObj.Id, oDado);
                                    }
                                }

                            }

                        }
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

        private async void ErroToFileDataErro(string[] listaCampos) {
            string textoCsv = "";
            foreach (string campo in listaCampos) {
                textoCsv += $"{campo},";
            }
            try {
                await SalvamentoDataErro(textoCsv);
            } catch (Exception ex01) {
                Thread.Sleep(50);
                try {
                    await SalvamentoDataErro(textoCsv);
                } catch (Exception ex02) {
                    Thread.Sleep(80);
                    try {
                        await SalvamentoDataErro(textoCsv);
                    } catch (Exception ex03) {
                        Thread.Sleep(100);
                        try {
                            await SalvamentoDataErro(textoCsv);
                        } catch (Exception ex04) {
                        }
                    }
                }
            }
        }

        private async Task SalvamentoDataErro(string textoCsv) {
            if (!File.Exists(fileErroCsv)) {
                using (var stream = new StreamWriter(fileErroCsv)) {
                    await stream.WriteLineAsync(textoCsv);
                }
            } else {
                using (StreamWriter sw = File.AppendText(fileErroCsv)) {
                    await sw.WriteLineAsync(textoCsv);
                }
            }
        }


        private bool OsDadosSaoIguais(DadosCovid oDadoArquivo, DadosCovid oDadoBanco) {
            bool igual = true;

            try {
                if (oDadoArquivo.city.Trim() != oDadoBanco.city.Trim()) {
                    return false;
                }
            } catch (System.Exception) {
                igual = false;
            }

            // Campos principais --------------------------------------------
            // public string city_ibge_code { get; set; }
            if (oDadoArquivo.city_ibge_code.Trim() != oDadoBanco.city_ibge_code.Trim()) {
                return false;
            }
            // public DateTime date { get; set; }
            if (oDadoArquivo.date.CompareTo(oDadoBanco.date) != 0) {
                return false;
            }
            // Campos principais --------------------------------------------

            // public string epidemiological_week { get; set; }
            try {
                if (oDadoArquivo.epidemiological_week.Trim() != oDadoBanco.epidemiological_week.Trim()) {
                    return false;
                }
            } catch (System.Exception) {
                igual = false;
            }

            // public long estimated_population { get; set; }
            try {
                if (oDadoArquivo.estimated_population != oDadoBanco.estimated_population) {
                    return false;
                }
            } catch (System.Exception) {
                igual = false;
            }
            // public long estimated_population_2019 { get; set; }
            try {
                if (oDadoArquivo.estimated_population_2019 != oDadoBanco.estimated_population_2019) {
                    return false;
                }
            } catch (System.Exception) {
                igual = false;
            }
            // public string is_last { get; set; }
            try {
                if (oDadoArquivo.is_last.Trim().ToLower() != oDadoBanco.is_last.Trim().ToLower()) {
                    return false;
                }
            } catch (System.Exception) {
                igual = false;
            }
            // public string is_repeated { get; set; }
            try {
                if (oDadoArquivo.is_repeated.Trim().ToLower() != oDadoBanco.is_repeated.Trim().ToLower()) {
                    return false;
                }
            } catch (System.Exception) {
                igual = false;
            }
            // public long city_ibglast_available_confirmede_code { get; set; }
            if (oDadoArquivo.city_ibglast_available_confirmede_code !=
                oDadoBanco.city_ibglast_available_confirmede_code) {
                return false;
            }

            // public double last_available_confirmed_per_100k_inhabitants { get; set; }
            // oDado.last_available_confirmed_per_100k_inhabitants = 
            //     Double.Parse(listaCampos[9].Trim());
            try {
                if (oDadoArquivo.last_available_confirmed_per_100k_inhabitants !=
                    oDadoBanco.last_available_confirmed_per_100k_inhabitants) {
                    return false;
                }
            } catch (System.Exception) {
                igual = false;
            }
            // public DateTime last_available_date { get; set; }
            if (oDadoArquivo.last_available_date.CompareTo(oDadoBanco.last_available_date) != 0) {
                return false;
            }
            // public double last_available_death_rate { get; set; }
            try {
                if (oDadoArquivo.last_available_death_rate !=
                    oDadoBanco.last_available_death_rate) {
                    return false;
                }
            } catch (System.Exception) {
                igual = false;
            }
            // public long last_available_deaths { get; set; }
            try {
                if (oDadoArquivo.last_available_deaths !=
                    oDadoBanco.last_available_deaths) {
                    return false;
                }
            } catch (System.Exception) {
                igual = false;
            }
            // public long order_for_place { get; set; }
            if (oDadoArquivo.order_for_place !=
                oDadoBanco.order_for_place) {
                return false;
            }
            // public string place_type { get; set; }
            if (oDadoArquivo.place_type.Trim().ToLower() != oDadoBanco.place_type.Trim().ToLower()) {
                return false;
            }
            // public string state { get; set; }
            if (oDadoArquivo.state.Trim().ToLower() != oDadoBanco.state.Trim().ToLower()) {
                return false;
            }
            // public long new_confirmed { get; set; }
            if (oDadoArquivo.new_confirmed != oDadoBanco.new_confirmed) {
                return false;
            }
            // public long new_deaths { get; set; }
            if (oDadoArquivo.new_deaths != oDadoBanco.new_deaths) {
                return false;
            }


            return igual;
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

        private async void SalvarCamposNovoArquivoCsvLimpo( string [] listaCampos ) {
            string textoCsv = "";
            foreach (string campo in listaCampos) {
                textoCsv += $"{campo},";
            }
            try {
                await EscritaDoArquivo(textoCsv);
            } catch (Exception ex01) {
                Thread.Sleep(50);
                try {
                    await EscritaDoArquivo(textoCsv);
                } catch (Exception ex02) {
                    Thread.Sleep(80);
                    try {
                        await EscritaDoArquivo(textoCsv);
                    } catch (Exception ex03) {
                        Thread.Sleep(100);
                        try {
                            await EscritaDoArquivo(textoCsv);
                        } catch (Exception ex04) {
                        }
                    }
                }
            }
        }

        private async Task EscritaDoArquivo(string textoCsv) {
            if (!File.Exists(fileCsvLimpo)) {
                using (var stream = new StreamWriter(fileCsvLimpo)) {
                    await stream.WriteLineAsync(textoCsv);
                }
            } else {
                using (StreamWriter sw = File.AppendText(fileCsvLimpo)) {
                    await sw.WriteLineAsync(textoCsv);
                }
            }
        }

        private async void ErroToFileDataErroLimpeza( string [] listaCampos ) {
            string textoCsv = "";
            foreach (string campo in listaCampos) {
                textoCsv += $"{campo},";
            }
            try {
                await EscritaDoArquivoLimpo(textoCsv);
            } catch (Exception ex01) {
                Thread.Sleep(50);
                try {
                    await EscritaDoArquivoLimpo(textoCsv);
                } catch (Exception ex02) {
                    Thread.Sleep(80);
                    try {
                        await EscritaDoArquivoLimpo(textoCsv);
                    } catch (Exception ex03) {
                        Thread.Sleep(100);
                        try {
                            await EscritaDoArquivoLimpo(textoCsv);
                        } catch (Exception ex04) {
                        }
                    }
                }
            }
        }

        private async Task EscritaDoArquivoLimpo(string textoCsv) {
            if (!File.Exists(fileErroCsvLimpeza)) {
                using (var stream = new StreamWriter(fileErroCsvLimpeza)) {
                    await stream.WriteLineAsync(textoCsv);
                }
            } else {
                using (StreamWriter sw = File.AppendText(fileErroCsvLimpeza)) {
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