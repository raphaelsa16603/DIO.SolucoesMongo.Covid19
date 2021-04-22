using System.Threading.Tasks;
using CovidBrDataSetFileProcess.Model;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CovidBrDataSetFileProcess.Controller
{
    public class DadosCovidController
    {
        private readonly Context _context;

        public DadosCovidController(Context context)
        {
            _context = context;
        }

        // POST
        public async void Cadastro(DadosCovid obj)
        {
            try
            {
                _context.Add(obj);
                await _context.SaveChangesAsync();
            }
            catch (SqliteException exSql)
            {
                throw new System.Exception(exSql.Message  + 
                " - Code: " + exSql.SqliteErrorCode + 
                " - Status: " + exSql.SqlState, exSql);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message, ex);
            }
            
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
            catch (SqliteException exSql)
            {
                throw new System.Exception(exSql.Message  + 
                " - Code: " + exSql.SqliteErrorCode + 
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
            catch (SqliteException exSql)
            {
                throw new System.Exception(exSql.Message  + 
                " - Code: " + exSql.SqliteErrorCode + 
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
            catch (SqliteException exSql)
            {
                throw new System.Exception(exSql.Message  + 
                " - Code: " + exSql.SqliteErrorCode + 
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