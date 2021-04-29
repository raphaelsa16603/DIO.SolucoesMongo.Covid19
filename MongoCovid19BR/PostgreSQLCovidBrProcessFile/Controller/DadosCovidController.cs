using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PostgreSQLCovidBrProcessFile.Model;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace PostgreSQLCovidBrProcessFile.Controller
{
    public class DadosCovidController
    {
        private readonly Context _context;

        public DadosCovidController(Context context)
        {
            _context = context;
        }

        // POST
        public async Task<int> Cadastro(DadosCovid obj)
        {
            int codigo = 0;
            try
            {
                _context.Add(obj);
                codigo = await _context.SaveChangesAsync();
            }
            catch (NpgsqlException exSql)
            {
                try
                {
                    // Sincrono...
                    codigo = _context.SaveChanges();    
                }
                catch (NpgsqlException exSql2)
                {
                    throw new System.Exception(exSql2.Message  + 
                    " - Code: " + exSql2.ErrorCode + 
                    " - Status: " + exSql2.SqlState, exSql);
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception(ex.Message, exSql);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message, ex);
            }
            
            return codigo;
        }

        // GET
        public async Task<DadosCovid> Get(int? id)
        {
            if (id == null)
            {
                throw new System.Exception("Sem o Id do registro");
            }

            DadosCovid Dados;
            try
            {
                Dados = await _context.OsDadosDoCovid.FindAsync(id);
            }
            catch (NpgsqlException exSql)
            {
                throw new System.Exception(exSql.Message  + 
                " - Code: " + exSql.ErrorCode + 
                " - Status: " + exSql.SqlState, exSql);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message, ex);
            }

            if (Dados == null)
            {
                throw new System.Exception("Registro não localizado");
            }

            return Dados;
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

                        
            DadosCovid Dados;
            try
            {
                //var query = from dados in _context.Set<DadosCovid>().Where;
                Dados = await _context.OsDadosDoCovid.SingleAsync
                (b => b.city_ibge_code == obj.city_ibge_code.Trim() &&
                      b.date.CompareTo(obj.date) == 0);
            }
            catch (NpgsqlException exSql)
            {
                throw new System.Exception(exSql.Message  + 
                " - Code: " + exSql.ErrorCode + 
                " - Status: " + exSql.SqlState, exSql);
            }
            catch (System.Exception) // ex)
            {
                //throw new System.Exception(ex.Message, ex);
                Dados = null;
            }

            if (Dados == null)
            {
                //throw new System.Exception("Registro não localizado");
                Dados = null;
            }

            return Dados;
        }

        // PUT
        public async Task<DadosCovid> Update(int id, DadosCovid obj)
        {
            if (id != obj.Id)
            {
                throw new System.Exception("Conflito entre o Id do registro e do objeto de dados");
            }

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (NpgsqlException exSql)
            {
                throw new System.Exception(exSql.Message  + 
                " - Code: " + exSql.ErrorCode + 
                " - Status: " + exSql.SqlState, exSql);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message, ex);
            }
            return await Get(id);
        }

        // DELETE
        public async Task<DadosCovid> Delete(int? id)
        {
            if (id == null)
            {
                throw new System.Exception("Sem o Id do registro");
            }

            var osDadosCovid = await _context.OsDadosDoCovid
                .FirstOrDefaultAsync(m => m.Id == id);
            if (osDadosCovid == null)
            {
                throw new System.Exception("Registro não localizado");
            }

            try
            {
                _context.OsDadosDoCovid.Remove(osDadosCovid);
                await _context.SaveChangesAsync();
            }
            catch (NpgsqlException exSql)
            {
                throw new System.Exception(exSql.Message  + 
                " - Code: " + exSql.ErrorCode + 
                " - Status: " + exSql.SqlState, exSql);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message, ex);
            }

            return osDadosCovid;
        }
    }
}