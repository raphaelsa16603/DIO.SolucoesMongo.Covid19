using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CovidBrApi.Data.Collections;
using CovidBrApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;


namespace CovidBrApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DadosCovidController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<DadosCovid> _dadosCovidCollection;

        public DadosCovidController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _dadosCovidCollection = _mongoDB.DB.GetCollection<DadosCovid>
                                       (typeof(DadosCovid).Name.Trim());//.Name.ToLower());
        }


        [HttpGet("{city_ibge_code}")]
        public async Task<IActionResult> ObterDadosCovid(string city_ibge_code)
        {
            var filter = Builders<DadosCovid>.Filter.Eq
                    (inf => inf.city_ibge_code, city_ibge_code);
            try
            {
                var oDadosCovid = await  _dadosCovidCollection.FindAsync(filter);
                var Dados = oDadosCovid.ToList();
            
                return Ok(Dados);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Banco de Dados Falhou : {ex.Message}");
            }
        }


        [HttpDelete("{city_ibge_code},{dia},{mes},{ano}")]
        public async void Delete(string city_ibge_code, 
                    string dia, string mes, string ano)
        {
            var filters = new List<FilterDefinition<DadosCovid>>();
            // Especificidade do MongoDB por causa do operador Gt e Lt, 
            // pois não contempla o igual, eis que a 
            // consulta da data tem que
            // ser nos dias anteriores e posterior 
            var DateFilter = new DateTime(Int32.Parse(ano), Int32.Parse(mes), Int32.Parse(dia)).AddDays(-1);
            var DateFilterEnd = new DateTime(Int32.Parse(ano), Int32.Parse(mes), Int32.Parse(dia)).AddDays(1);
            var filter1 = Builders<DadosCovid>.Filter.Eq
                    (inf => inf.city_ibge_code, city_ibge_code);
            var filter2 = Builders<DadosCovid>.Filter.Gt
                    (inf => inf.date, DateFilter);        
            var filter3 = Builders<DadosCovid>.Filter.Lt
                    (inf => inf.date, DateFilterEnd);
            filters.Add(filter1);
            filters.Add(filter2);
            filters.Add(filter3);
            var complexFilter = Builders<DadosCovid>.Filter.And(filters);

            try
            {
                var result = await  _dadosCovidCollection.DeleteOneAsync(complexFilter);
            }
            catch (System.Exception) //(System.Exception ex)
            {
                //$"Banco de Dados Falhou : {ex.Message}"
            }
        }

        
        [HttpPut("{city_ibge_code},{dia},{mes},{ano}")]
        public async Task<IActionResult> AtualizarDados
                    (string city_ibge_code, 
                    string dia, string mes, string ano,
                    [FromBody] DadosCovidDto dto)
        {
            var filters = new List<FilterDefinition<DadosCovid>>();
            // var DateFilter = new DateTime(dto.date.Day, dto.date.Month, dto.date.Year).AddDays(-1);
            // var DateFilterEnd = new DateTime(dto.date.Day, dto.date.Month, dto.date.Year);
            /* Registro de teste
            {
                "city": "Rio Branco",
                "city_ibge_code": "1200401",
                "date": "2020-03-17T00:00:00.000Z",
                "epidemiological_week": "202012",
                "estimated_population": 413418,
                "estimated_population_2019": 407319,
                "is_last": "False",
                "is_repeated": "False",
                "city_ibglast_available_confirmede_code": 3,
                "last_available_confirmed_per_100k_inhabitants": 0.72566,
                "last_available_date": "2020-03-17T00:00:00.000Z",
                "last_available_death_rate": 0,
                "last_available_deaths": 0,
                "order_for_place": 1,
                "place_type": "city",
                "state": "AC",
                "new_confirmed": 3,
                "new_deaths": 0,
                "uId": "607a509f7369c0a5510f54fc"
            }
            */
            // Especificidade do MongoDB por causa do operador Gt e Lt, 
            // pois não contempla o igual, eis que a 
            // consulta da data tem que
            // ser nos dias anteriores e posterior 
            var DateFilter = new DateTime(Int32.Parse(ano), Int32.Parse(mes), Int32.Parse(dia)).AddDays(-1);
            var DateFilterEnd = new DateTime(Int32.Parse(ano), Int32.Parse(mes), Int32.Parse(dia)).AddDays(1);
            var filter1 = Builders<DadosCovid>.Filter.Eq
                    (inf => inf.city_ibge_code, dto.city_ibge_code);
            var filter2 = Builders<DadosCovid>.Filter.Gt
                    (inf => inf.date, DateFilter);        
            var filter3 = Builders<DadosCovid>.Filter.Lt
                    (inf => inf.date, DateFilterEnd);
            filters.Add(filter1);
            filters.Add(filter2);
            filters.Add(filter3);
            var complexFilter = Builders<DadosCovid>.Filter.And(filters);

            UpdateDefinition<DadosCovid> update = null;

            if (dto.city != null)
                update = Builders<DadosCovid>.Update
                        .Set( s => s.city, dto.city);
            
            if (dto.city_ibge_code != null)
                update = Builders<DadosCovid>.Update
                        .Set( s => s.city_ibge_code, dto.city_ibge_code);

            update = Builders<DadosCovid>.Update
                        .Set( s => s.city_ibglast_available_confirmede_code, 
                        dto.city_ibglast_available_confirmede_code);

            update = Builders<DadosCovid>.Update
                        .Set( s => s.date, 
                        dto.date);
            
            update = Builders<DadosCovid>.Update
                        .Set( s => s.epidemiological_week, 
                        dto.epidemiological_week);
            
            update = Builders<DadosCovid>.Update
                        .Set( s => s.estimated_population, 
                        dto.estimated_population);
                        
            update = Builders<DadosCovid>.Update
                        .Set( s => s.estimated_population_2019, 
                        dto.estimated_population_2019);
            
            update = Builders<DadosCovid>.Update
                        .Set( s => s.is_last, 
                        dto.is_last);
            
            update = Builders<DadosCovid>.Update
                        .Set( s => s.is_repeated, 
                        dto.is_repeated);

            update = Builders<DadosCovid>.Update
                        .Set( s => s.last_available_confirmed_per_100k_inhabitants, 
                        dto.last_available_confirmed_per_100k_inhabitants);

            // dto.last_available_date,
            update = Builders<DadosCovid>.Update
                        .Set( s => s.last_available_date, 
                        dto.last_available_date);
            // dto.last_available_death_rate,
            update = Builders<DadosCovid>.Update
                        .Set( s => s.last_available_death_rate, 
                        dto.last_available_death_rate);
            // dto.last_available_deaths,
            update = Builders<DadosCovid>.Update
                        .Set( s => s.last_available_deaths, 
                        dto.last_available_deaths);
            // dto.new_confirmed,
            update = Builders<DadosCovid>.Update
                        .Set( s => s.new_confirmed, 
                        dto.new_confirmed);
            // dto.new_deaths,
            update = Builders<DadosCovid>.Update
                        .Set( s => s.new_deaths, 
                        dto.new_deaths);
            // dto.order_for_place,
            update = Builders<DadosCovid>.Update
                        .Set( s => s.order_for_place, 
                        dto.order_for_place);
            // dto.place_type,
            update = Builders<DadosCovid>.Update
                        .Set( s => s.place_type, 
                        dto.place_type);
            // dto.state, 
            update = Builders<DadosCovid>.Update
                        .Set( s => s.state, 
                        dto.state);
            // dto.uId
            update = Builders<DadosCovid>.Update
                        .Set( s => s.uId, 
                        dto.uId);

            try
            {
                if(update != null)
                {
                    var result = await  _dadosCovidCollection.UpdateOneAsync
                                                (complexFilter, update);
                }
                return StatusCode(201, "Um Dado do Covid foi atualizado com sucesso");
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Banco de Dados Falhou : {ex.Message}");
            }
        }
        

        [HttpPost]
        public async Task<IActionResult> SalvarDados([FromBody] DadosCovidDto dto)
        {
            var dados = new DadosCovid(
                dto.city,
                dto.city_ibge_code, 
                dto.city_ibglast_available_confirmede_code, 
                dto.date, 
                dto.epidemiological_week, 
                dto.estimated_population, 
                dto.estimated_population_2019, 
                dto.is_last, 
                dto.is_repeated, 
                dto.last_available_confirmed_per_100k_inhabitants,
                dto.last_available_date,
                dto.last_available_death_rate,
                dto.last_available_deaths,
                dto.new_confirmed,
                dto.new_deaths,
                dto.order_for_place,
                dto.place_type,
                dto.state, 
                dto.uId
                );
            try
            {
                await _dadosCovidCollection.InsertOneAsync(dados);
                
                return StatusCode(201, "Dados do Covid adicionado com sucesso");
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Banco de Dados Falhou : {ex.Message}");
            }
        }
    }
}