using System;

namespace CovidBrDataSetFileProcess.Model
{
    public class DadosCovid
    {
        public int Id { get; set; }
        public string city { get; set; }
        public string city_ibge_code { get; set; }

        public DateTime date { get; set; }

        public string epidemiological_week { get; set; }

        public long estimated_population { get; set; }
        public long estimated_population_2019 { get; set; }

        public string is_last { get; set; }
        public string is_repeated { get; set; }
        public long city_ibglast_available_confirmede_code { get; set; }

        public double last_available_confirmed_per_100k_inhabitants { get; set; }

        public DateTime last_available_date { get; set; }

        public double last_available_death_rate { get; set; }

        public long last_available_deaths { get; set; }
        public long order_for_place { get; set; }

        public string place_type { get; set; }

        public string state { get; set; }


        public long new_confirmed { get; set; }
        public long new_deaths { get; set; }

        public string uId {get; set;}

        public bool dadosNovos {get; set;}

        public bool dadosAtualizados {get; set;}

        public override string ToString()
        {
            return $"{{\n" + 
                   $"\"city\": \"{city}\",\n" +
                   $"\"city_ibge_code\": \"{city_ibge_code}\",\n" +
                   $"\"date\": \"{date}\",\n" +
                   $"\"epidemiological_week\": \"{epidemiological_week}\",\n" +
                   $"\"estimated_population\": \"{estimated_population}\",\n" +
                   $"\"estimated_population_2019\": \"{estimated_population_2019}\",\n" +
                   $"\"is_last\": \"{is_last}\",\n" +
                   $"\"is_repeated\": \"{is_repeated}\",\n" +
                   $"\"city_ibglast_available_confirmede_code\": \"{city_ibglast_available_confirmede_code}\",\n" +
                   $"\"last_available_confirmed_per_100k_inhabitants\": \"{last_available_confirmed_per_100k_inhabitants}\",\n" +
                   $"\"last_available_date\": \"{last_available_date}\",\n" +
                   $"\"last_available_death_rate\": \"{last_available_death_rate}\",\n" +
                   $"\"last_available_deaths\": \"{last_available_deaths}\",\n" +
                   $"\"order_for_place\": \"{order_for_place}\",\n" +
                   $"\"place_type\": \"{place_type}\",\n" +
                   $"\"state\": \"{state}\",\n" +
                   $"\"new_confirmed\": \"{new_confirmed}\",\n" +
                   $"\"new_deaths\": \"{new_deaths}\"\n" +
                   "}";
 
       }
    }
}