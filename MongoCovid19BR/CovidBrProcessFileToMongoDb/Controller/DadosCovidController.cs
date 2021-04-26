using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CovidBrProcessFileToMongoDb.Lib.ToolsLogs;
using CovidBrProcessFileToMongoDb.Model;
using CovidBrProcessFileToMongoDb.Models;
using MongoDB.Driver;

namespace CovidBrProcessFileToMongoDb.Controller
{
    public class DadosCovidController
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<DadosCovid> _dadosCovidCollection;

        public DadosCovidController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _dadosCovidCollection = _mongoDB.DB.GetCollection<DadosCovid>
                                       (typeof(DadosCovid).Name.ToLower()); // TODO: Modificar?
        }

        // POST
        public async Task<int> Cadastro(DadosCovid dto)
        {
            int codigo = 0;
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
                codigo = 0;
                return codigo;
            }
            catch (System.Exception ex)
            {
                LogTools.LogErroToFile($" Erro no cadastro {ex.Message}", ex.StackTrace);
                codigo = -1;
                return codigo;
            }
            
        }

        public async Task<List<DadosCovid>> ObterDadosCovid(string city_ibge_code)
        {
            var filter = Builders<DadosCovid>.Filter.Eq
                    (inf => inf.city_ibge_code, city_ibge_code);
            try
            {
                var oDadosCovid = await  _dadosCovidCollection.FindAsync(filter);
                var Dados = oDadosCovid.ToList();
            
                return Dados;
            }
            catch (System.Exception ex)
            {
                LogTools.LogErroToFile
                    ($" Erro na Consulta por city_ibge_code {ex.Message}", ex.StackTrace);
                return null;
            }
        }

        // GET
        public async Task<DadosCovid> Get(string uId)
        {
            if (uId == null)
            {
                throw new System.Exception("Sem o Id do registro");
            }
            var filter = Builders<DadosCovid>.Filter.Eq
                    (inf => inf.uId, uId);
            try
            {
                var oDadosCovid = await  _dadosCovidCollection.FindAsync(filter);
                var Dados = oDadosCovid.ToList();
            
                return Dados[0];
            }
            catch (System.Exception ex)
            {
                LogTools.LogErroToFile
                    ($" Erro na Consulta por city_ibge_code {ex.Message}", ex.StackTrace);
                return null;
            }
        }

        public async Task<DadosCovid> Pesquisa(DadosCovid obj)
        {
            if (obj == null)
            {
                throw new System.Exception("Sem o Objeto de Dados para pesquisa do registro");
            }

            if( obj.city_ibge_code.Trim().Equals(""))
            {
                throw new System.Exception("Sem  city_ibge_code para pesquisa do registro");
            }

                        
            DadosCovid Dados = null;
            var filters = new List<FilterDefinition<DadosCovid>>();
            var DateFilter = new DateTime(obj.date.Year, obj.date.Month, obj.date.Day).AddDays(-1);
            var DateFilterEnd = new DateTime(obj.date.Year, obj.date.Month, obj.date.Day);
            var filter1 = Builders<DadosCovid>.Filter.Eq
                    (inf => inf.city_ibge_code, obj.city_ibge_code);
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
                // Não estou conseguindo debugar o erro da pesquisa
                // Trava tudo e não passa nem no catch
                var oDadosCovid = await  _dadosCovidCollection.FindAsync(complexFilter);
                
                var DadosList = oDadosCovid.ToList();
                Dados = DadosList[0];
            
                return Dados;
            }
            catch (System.Exception ex)
            {
                LogTools.LogErroToFile
                    ($" Erro na Pesquisa pelo Objeto {ex.Message}", ex.StackTrace);
            }

            return Dados;
        }

        // PUT
        public async Task<DadosCovid> Update(DadosCovid dto)
        {
            if (dto == null)
            {
                throw new System.Exception("Conflito objeto de dados nulo (NULL)");
            }

            var filters = new List<FilterDefinition<DadosCovid>>();
            var DateFilter = new DateTime(dto.date.Year, dto.date.Month, dto.date.Day).AddDays(-1);
            var DateFilterEnd = new DateTime(dto.date.Year, dto.date.Month, dto.date.Day);
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
                
            }
            catch (System.Exception ex)
            {
                LogTools.LogErroToFile
                    ($" Erro na Atualização dos dados do Objeto {ex.Message}", ex.StackTrace);
            }
            return await Get(dto.uId);
        }

        // DELETE
        public async Task<DadosCovid> DeleteByUid(string uId)
        {
            if (uId == null)
            {
                throw new System.Exception("Sem o Id do registro");
            }

            DadosCovid dados = await Get(uId);

            var filter1 = Builders<DadosCovid>.Filter.Eq
                    (inf => inf.uId, uId);

            try
            {
                var result = await  _dadosCovidCollection.DeleteOneAsync(filter1);
            }
            catch (System.Exception ex)
            {
                LogTools.LogErroToFile
                    ($" Erro na remoção dos dados do registro de uId [{uId}]: {ex.Message}", ex.StackTrace);
            }

            return dados;
        }

        public async Task<DadosCovid> Delete(DadosCovid dto)
        {
            if (dto == null)
            {
                throw new System.Exception("Sem o objeto de dados");
            }

            var filters = new List<FilterDefinition<DadosCovid>>();
            var DateFilter = new DateTime(dto.date.Year, dto.date.Month, dto.date.Day).AddDays(-1);
            var DateFilterEnd = new DateTime(dto.date.Year, dto.date.Month, dto.date.Day);
            var filter1 = Builders<DadosCovid>.Filter.Eq
                    (inf => inf.city_ibge_code, dto.city_ibge_code);
            var filter2 = Builders<DadosCovid>.Filter.Gt
                    (inf => inf.date, DateFilter);        
            var filter3 = Builders<DadosCovid>.Filter.Lt
                    (inf => inf.date, DateFilterEnd);
            var complexFilter = Builders<DadosCovid>.Filter.And(filters);
            try
            {
                var result = await  _dadosCovidCollection.DeleteOneAsync(complexFilter);
            }
            catch (System.Exception ex)
            {
                LogTools.LogErroToFile
                    ($" Erro na remoção dos dados do Objeto {dto.uId}: {ex.Message}", ex.StackTrace);
            }

            return dto;
        }
    }
}