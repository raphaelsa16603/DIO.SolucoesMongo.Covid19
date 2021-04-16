using System.Threading.Tasks;
using Api.Data.Collections;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>
                                       (typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public async Task<IActionResult> SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, 
            dto.Latitude, dto.Longitude, 
            dto.DataObito);
            try
            {
                await _infectadosCollection.InsertOneAsync(infectado);
                
                return StatusCode(201, "Infectado adicionado com sucesso");
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Banco de Dados Falhou : {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterInfectados()
        {
            try
            {
                var oInfectados = await  _infectadosCollection.FindAsync
                                    (Builders<Infectado>.Filter.Empty);
                var infectados = oInfectados.ToList();
            
                return Ok(infectados);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Banco de Dados Falhou : {ex.Message}");
            }
        }

        [HttpGet("{uid}")]
        public async Task<IActionResult> Get(string uid)
        {
            var filter = Builders<Infectado>.Filter.Eq(inf => inf.uId, uid);
            try
            {
                var obj = await  _infectadosCollection.FindAsync(filter);
                var results = obj.ToList();
                return Ok(results);    
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Banco de Dados Falhou : {ex.Message}");
            }
        }


        // PUT api/values/5
        [HttpPut("{uid}")]
        public async void Put(string uid, [FromBody] InfectadoDto dto)
        {
            var filter = Builders<Infectado>.Filter.Eq(inf => inf.uId, uid);
            UpdateDefinition<Infectado> update = null;
            if (dto.DataNascimento != null)
                update = Builders<Infectado>.Update
                        .Set( s => s.DataNascimento, dto.DataNascimento);
            
            if (dto.DataObito != null)
                update = Builders<Infectado>.Update
                        .Set( s => s.DataObito, dto.DataObito);
            if (dto.Sexo != null)
                update = Builders<Infectado>.Update
                        .Set( s => s.Sexo, dto.Sexo);
                        
            // update = Builders<Infectado>.Update
            //             .Set( s => s.Localizacao.Longitude, dto.Longitude);
            // update = Builders<Infectado>.Update            
            //              .Set( s => s.Localizacao.Latitude, dto.Latitude);
            
            try
            {
                if(update != null)
                {
                    var result = await  _infectadosCollection.UpdateOneAsync(filter, update);
                }
            }
            catch (System.Exception ex) //(System.Exception ex)
            {
                //$"Banco de Dados Falhou : {ex.Message}"
            }
        }

        // DELETE api/values/5
        [HttpDelete("{uid}")]
        public async void Delete(string uid)
        {
            var filter = Builders<Infectado>.Filter.Eq(inf => inf.uId, uid);
            try
            {
                var result = await  _infectadosCollection.DeleteOneAsync(filter);
            }
            catch (System.Exception) //(System.Exception ex)
            {
                //$"Banco de Dados Falhou : {ex.Message}"
            }
        }
    }
}
