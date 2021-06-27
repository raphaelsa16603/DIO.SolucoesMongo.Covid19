using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlCovidBrFileProcess.Model
{
    public class Estado
    {
        public int Id { get; set; }
        public string state { get; set; }
        public string city_ibge_code { get; set; }
        public string place_type { get; set; }
        public string uId { get; set; }
    }
}
