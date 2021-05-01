using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IncrementalDataFileProcessPostgreSql.Model;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace IncrementalDataFileProcessPostgreSql.Controller
{
    public class DadosCovidController : IDisposable
    {
        private readonly Context _context;
        private int _registros;
        // Extranho qualquer volor diferente de 1000, não é realizado o SaveChanges!!
        private readonly int _cache = 1000;

        // public DadosCovidController(Context context)
        // {
        //     _context = context;
        // }

        // Singleton
        private DadosCovidController(Context context) 
        {
            _context = context;
         }

        // The Singleton's instance is stored in a static field.
        private static DadosCovidController _instance;

        // This is the static method that controls the access to the singleton
        // instance.
        public static DadosCovidController GetInstance(Context context)
        {
            if (_instance == null)
            {
                _instance = new DadosCovidController(context);
            }
            return _instance;
        }

        // POST
        public int Cadastro(DadosCovid obj)
        {
            int codigo = 0;
            try
            {
                _context.Add(obj);
                _registros++;
                // Uso de cache de dados para depois fazer atualização no DB
                if((_registros%_cache) == 0)
                    codigo = _context.SaveChanges();
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

        public int CadastroSimples(DadosCovid obj)
        {
            int codigo = 0;
            try
            {
                _context.Add(obj);
                codigo = _context.SaveChanges();
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
        public DadosCovid Get(int? id)
        {
            if (id == null)
            {
                throw new System.Exception("Sem o Id do registro");
            }

            DadosCovid Dados;
            try
            {
                Dados = _context.OsDadosDoCovid.Find(id);
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

        public DadosCovid Pesquisa(DadosCovid obj)
        {
            // PesquisaCaller caller = new PesquisaCaller(PesquisaAsync);
            // IAsyncResult result = caller.BeginInvoke(obj, null, null);
            // var Dados = caller.EndInvoke(result);
            var Dados = PesquisaAsync(obj).Result;

            return Dados;
        }

        // public delegate Task<DadosCovid> PesquisaCaller(DadosCovid obj);
        public async Task<DadosCovid> PesquisaAsync(DadosCovid obj)
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
                // object [] param = { obj.city_ibge_code.Trim(), obj.date.ToString("yyyy-MM-dd")};
                // var consulta = _context.OsDadosDoCovid.FromSqlRaw
                // ("Select * from public.\"OsDadosCovid\" " + 
                //  "Where city_ibge_code = '{0}' and " + 
                //  " date = '{1}'", param);
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
        public DadosCovid Update(int id, DadosCovid obj)
        {
            if (id != obj.Id)
            {
                throw new System.Exception("Conflito entre o Id do registro e do objeto de dados");
            }

            try
            {
                _context.Update(obj);
                _context.SaveChanges();
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
            return Get(id);
        }

        public DadosCovid Delete(int? id)
        {
            var dados = DeleteAsync(id).Result;

            return dados;
        }
        // DELETE
        public async Task<DadosCovid> DeleteAsync(int? id)
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

        public void Dispose()
        {
            // Salva o que ainda não foi salvo no chache
            try
            {
                _context.SaveChanges();
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
        }
    }
}