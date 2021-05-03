using Microsoft.EntityFrameworkCore;
using IncrementalUfFileProcessSQLite.Model;

namespace IncrementalUfFileProcessSQLite.Model
{
    public class Context : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=../CovidBrDataSetFileProcess/DadosCovid19BR.db");
        }

        public DbSet<DadosCovid> OsDadosDoCovid { get; set; }

    }
}