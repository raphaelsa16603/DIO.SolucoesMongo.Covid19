using System;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Api.Data.Collections
{
    public class Infectado
    {
        public Infectado(DateTime dataNascimento, string sexo, 
                    double latitude, double longitude, 
                    DateTime dataObito)
        {
            this.DataNascimento = dataNascimento;
            this.Sexo = sexo;
            this.Localizacao = new GeoJson2DGeographicCoordinates(longitude, latitude);
            this.DataObito = dataObito;
            this.uId = GerarUId();
        }
        
        public string uId { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }

        public DateTime DataObito { get; set; }
        public GeoJson2DGeographicCoordinates Localizacao { get; set; }

        public string GerarUId()
        { 
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
                        
            return myuuidAsString;
        }

        public int GerarId()
        { 
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();
            string numerosUid = "";
            // Pegar só os números
            foreach(char l in myuuidAsString)
            {
                if(l >= '0' && l <= '9')
                    numerosUid += l;
            }

            int iRet = 0;
            try
            {
                iRet = int.Parse(numerosUid);    
            }
            catch (Exception)
            {
                iRet = 1;
            }
            
            return iRet;
        }
    }
}