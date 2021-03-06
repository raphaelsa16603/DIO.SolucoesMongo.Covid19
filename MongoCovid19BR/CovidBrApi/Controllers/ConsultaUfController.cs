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
    public class ConsultaUfController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<DadosCovid> _dadosCovidCollection;

        public ConsultaUfController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _dadosCovidCollection = _mongoDB.DB.GetCollection<DadosCovid>
                                       (typeof(DadosCovid).Name.Trim());//.Name.ToLower());
        }

        [HttpGet("{uf},{dia},{mes},{ano}")]
        public async Task<IActionResult> ObterDadosCovid
                    (string uf, 
                    string dia, string mes, string ano)
        {
            var filters = new List<FilterDefinition<DadosCovid>>();
            // Especificidade do MongoDB por causa do operador Gt e Lt, 
            // pois não contempla o igual, eis que a 
            // consulta da data tem que
            // ser nos dias anterior e posterior 
            var DateFilter = new DateTime(Int32.Parse(ano), Int32.Parse(mes), Int32.Parse(dia)).AddDays(-1);
            var DateFilterEnd = new DateTime(Int32.Parse(ano), Int32.Parse(mes), Int32.Parse(dia)).AddDays(1);
            var filter1 = Builders<DadosCovid>.Filter.Eq
                    (inf => inf.state, uf);
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