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
                                       (typeof(DadosCovid).Name.ToLower());
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
                dto.state
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