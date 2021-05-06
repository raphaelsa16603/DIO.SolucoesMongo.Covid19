using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CovidBrProcessFileToMongoDb.Data.Collections
{
    public class DadosCovid
    {
        // Gerando erro na serialização ...
        [BsonId]
        public ObjectId Id { get; set; }
        
        public string city { get; set; }
        public string city_ibge_code { get; set; }

        public DateTime date { get; set; }

        public string epidemiological_week { get; set; }

        public long estimated_population { get; set; }
        public long estimated_population_2019 { get; set; }

        public string is_last { get; set; }
        public string is_repeated { get; set; }
        public long city_ibglast_available_confirmede_code { get; set; }

        //public double last_available_confirmed_per_100k_inhabitants { get; set; }
        public double last_available_confirmed_per_100k_inhabitants { get; set; }

        //??? Campo extra no banco de dados???
        public double last_available_confirmed { get; set; }

        public DateTime last_available_date { get; set; }

        public double last_available_death_rate { get; set; }

        public long last_available_deaths { get; set; }
        public long order_for_place { get; set; }

        public string place_type { get; set; }

        public string state { get; set; }


        public long new_confirmed { get; set; }
        public long new_deaths { get; set; }

        public string uId {get; set;}

        public DadosCovid(
                string city,
                string city_ibge_code, 
                long city_ibglast_available_confirmede_code, 
                DateTime date, 
                string epidemiological_week, 
                long estimated_population, 
                long estimated_population_2019, 
                string is_last, 
                string is_repeated, 
                double last_available_confirmed,
                double last_available_confirmed_per_100k_inhabitants,
                DateTime last_available_date,
                double last_available_death_rate,
                long last_available_deaths,
                long new_confirmed,
                long new_deaths,
                long order_for_place,
                string place_type,
                string state
                )
        { 

          this.city = city;
          this.city_ibge_code = city_ibge_code;
          this.city_ibglast_available_confirmede_code = city_ibglast_available_confirmede_code;
          this.date = date;
          this.epidemiological_week = epidemiological_week;
          this.estimated_population = estimated_population; 
          this.estimated_population_2019 = estimated_population_2019;
          this.is_last = is_last;
          this.is_repeated = is_repeated;
          this.last_available_confirmed = last_available_confirmed;
          this.last_available_confirmed_per_100k_inhabitants = last_available_confirmed_per_100k_inhabitants;
          this.last_available_date = last_available_date;
          this.last_available_death_rate = last_available_death_rate;
          this.last_available_deaths = last_available_deaths;
          this.new_confirmed = new_confirmed;
          this.new_deaths = new_deaths;
          this.order_for_place = order_for_place;
          this.place_type = place_type;
          this.state = state;
          this.uId = "";

        }

        public DadosCovid(
                string city,
                string city_ibge_code, 
                long city_ibglast_available_confirmede_code, 
                DateTime date, 
                string epidemiological_week, 
                long estimated_population, 
                long estimated_population_2019, 
                string is_last, 
                string is_repeated, 
                double last_available_confirmed,
                double last_available_confirmed_per_100k_inhabitants,
                DateTime last_available_date,
                double last_available_death_rate,
                long last_available_deaths,
                long new_confirmed,
                long new_deaths,
                long order_for_place,
                string place_type,
                string state, 
                string uId
                )
        { 

          this.city = city;
          this.city_ibge_code = city_ibge_code;
          this.city_ibglast_available_confirmede_code = city_ibglast_available_confirmede_code;
          this.date = date;
          this.epidemiological_week = epidemiological_week;
          this.estimated_population = estimated_population; 
          this.estimated_population_2019 = estimated_population_2019;
          this.is_last = is_last;
          this.is_repeated = is_repeated;
          this.last_available_confirmed = last_available_confirmed;
          this.last_available_confirmed_per_100k_inhabitants = last_available_confirmed_per_100k_inhabitants;
          this.last_available_date = last_available_date;
          this.last_available_death_rate = last_available_death_rate;
          this.last_available_deaths = last_available_deaths;
          this.new_confirmed = new_confirmed;
          this.new_deaths = new_deaths;
          this.order_for_place = order_for_place;
          this.place_type = place_type;
          this.state = state;
          this.uId = uId;

        }
    }
}