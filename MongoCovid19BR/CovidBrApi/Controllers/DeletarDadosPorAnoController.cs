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
    public class DeletarDadosPorAnoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<DadosCovid> _dadosCovidCollection;

        public DeletarDadosPorAnoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _dadosCovidCollection = _mongoDB.DB.GetCollection<DadosCovid>
                                       (typeof(DadosCovid).Name.Trim());//.Name.ToLower());
        }
        

        [HttpDelete("{ano}")]
        public async void Delete(string ano)
        {
            var filters = new List<FilterDefinition<DadosCovid>>();
            var DateFilter = new DateTime(Int32.Parse(ano), 01, 01).AddDays(-1);
            var DateFilterEnd = new DateTime(Int32.Parse(ano), 12, 31);
            var filter2 = Builders<DadosCovid>.Filter.Gt
                    (inf => inf.date, DateFilter);        
            var filter3 = Builders<DadosCovid>.Filter.Lt
                    (inf => inf.date, DateFilterEnd);
            filters.Add(filter2);
            filters.Add(filter3);
            var complexFilter = Builders<DadosCovid>.Filter.And(filters);

            try
            {
                var result = await  _dadosCovidCollection.DeleteManyAsync(complexFilter);
            }
            catch (System.Exception) //(System.Exception ex)
            {
                //$"Banco de Dados Falhou : {ex.Message}"
            }
        }
    }
}