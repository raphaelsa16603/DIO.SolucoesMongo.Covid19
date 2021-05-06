using System;
using MongoDbAtlasDataUfFileProcess.Data.Collections;
using System.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace MongoDbAtlasDataUfFileProcess.Data
{
    public class MongoDB
    {
        public IMongoDatabase DB { get; }

        public MongoDB()
        {
            try
            {
                var settings = MongoClientSettings.FromUrl
                    (new MongoUrl(ConfigurationManager.AppSettings["ConnectionString"]));
                var client = new MongoClient(settings);
                DB = client.GetDatabase(ConfigurationManager.AppSettings["NomeBanco"]);
                MapClasses();
            }
            catch (Exception ex)
            {
                throw new MongoException("It was not possible to connect to MongoDB", ex);
            }
        }

        private void MapClasses()
        {
            var conventionPack = 
                new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            if (!BsonClassMap.IsClassMapRegistered(typeof(DadosCovid)))
            {
                BsonClassMap.RegisterClassMap<DadosCovid>(i =>
                {
                    i.AutoMap();
                    // i.MapMember(c => c.city);
                    // i.MapMember(c => c.city_ibge_code);
                    // i.MapMember(c => c.city_ibglast_available_confirmede_code);
                    // i.MapMember(c => c.date);
                    // i.MapMember(c => c.epidemiological_week);
                    // i.MapMember(c => c.estimated_population);
                    // i.MapMember(c => c.estimated_population_2019);
                    // i.MapMember(c => c.is_last);
                    // i.MapMember(c => c.is_repeated);
                    // i.MapMember(c => c.last_available_confirmed_per_100k_inhabitants);
                    // i.MapMember(c => c.last_available_date);
                    // i.MapMember(c => c.last_available_death_rate);
                    // i.MapMember(c => c.last_available_deaths);
                    // i.MapMember(c => c.new_confirmed);
                    // i.MapMember(c => c.new_deaths);
                    // i.MapMember(c => c.order_for_place);
                    // i.MapMember(c => c.place_type);
                    // i.MapMember(c => c.state);
                    i.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}