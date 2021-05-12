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
    public class ConsultaIbgeComplexaController : ControllerBase
    {
         Data.MongoDB _mongoDB;
        IMongoCollection<DadosCovid> _dadosCovidCollection;

        public ConsultaIbgeComplexaController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _dadosCovidCollection = _mongoDB.DB.GetCollection<DadosCovid>
                                       (typeof(DadosCovid).Name.Trim());//.Name.ToLower());
        }

        [HttpGet("{city_ibge_code},{mesinit},{anoinit},{mesfim},{anofim}")]
        public async Task<IActionResult> ObterDadosCovid
                    (string city_ibge_code, string mesinit, string anoinit,
                    string mesfim, string anofim)
        {
            var filters = new List<FilterDefinition<DadosCovid>>();
            // Na data final tem que acrescentar mais um mês para a consulta retornar o último mes também.
            // Por alguma especificiade do MongoDB 
            var DateFilterFim = new DateTime(Int32.Parse(anofim), Int32.Parse(mesfim), 01).AddMonths(1);
            var DateFilterInit = new DateTime(Int32.Parse(anoinit), Int32.Parse(mesinit), 01);
            var filter1 = Builders<DadosCovid>.Filter.Eq
                    (inf => inf.city_ibge_code, city_ibge_code);
            var filter2 = Builders<DadosCovid>.Filter.Lt
                    (inf => inf.date, DateFilterFim);
            var filter3 = Builders<DadosCovid>.Filter.Gte
                    (inf => inf.date, DateFilterInit);
            filters.Add(filter1);
            filters.Add(filter2);
            filters.Add(filter3);
            var complexFilter = Builders<DadosCovid>.Filter.And(filters);
            
            try
            {
                var oDadosCovid = await  _dadosCovidCollection.FindAsync(complexFilter);
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
    }
}