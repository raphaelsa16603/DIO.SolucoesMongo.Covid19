using Microsoft.EntityFrameworkCore;
using CursoMVC.Models;
using CovidBrDataSetFileProcess.Model;

namespace CursoMVC.Models
{
    public class Context : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DadosCovid19BR.db");
        }

        public DbSet<DadosCovid> OsDadosDoCovid { get; set; }

    }
}