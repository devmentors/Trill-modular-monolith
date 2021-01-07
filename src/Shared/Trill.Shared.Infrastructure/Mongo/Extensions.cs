using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Trill.Shared.Infrastructure.Mongo.Factories;
using Trill.Shared.Infrastructure.Mongo.Repositories;
using Trill.Shared.Infrastructure.Mongo.Seeders;

namespace Trill.Shared.Infrastructure.Mongo
{
    public static class Extensions
    {
        private static bool _conventionsRegistered;
        private const string SectionName = "mongo";
        
        public static IServiceCollection AddMongoRepository<TEntity, TIdentifiable>(this IServiceCollection services,
            string collectionName)
            where TEntity : IIdentifiable<TIdentifiable>
        {
            services.AddTransient<IMongoRepository<TEntity, TIdentifiable>>(sp =>
            {
                var database = sp.GetService<IMongoDatabase>();
                return new MongoRepository<TEntity, TIdentifiable>(database, collectionName);
            });

            return services;
        }

        internal static IServiceCollection AddMongo(this IServiceCollection services, string sectionName = SectionName,
            Type seederType = null)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                sectionName = SectionName;
            }

            var mongoOptions = services.GetOptions<MongoOptions>(sectionName);
            services.AddSingleton(mongoOptions);
            services.AddSingleton<IMongoClient>(sp =>
            {
                var options = sp.GetService<MongoOptions>();
                return new MongoClient(options.ConnectionString);
            });
            services.AddTransient(sp =>
            {
                var options = sp.GetService<MongoOptions>();
                var client = sp.GetService<IMongoClient>();
                return client.GetDatabase(options.Database);
            });
            services.AddTransient<IMongoSessionFactory, MongoSessionFactory>();

            if (seederType is null)
            {
                services.AddTransient<IMongoDbSeeder, MongoDbSeeder>();
            }
            else
            {
                services.AddTransient(typeof(IMongoDbSeeder), seederType);
            }

            if (!_conventionsRegistered)
            {
                RegisterConventions();
            }

            return services;
        }

        private static void RegisterConventions()
        {
            _conventionsRegistered = true;
            BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(decimal?),
                new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
            ConventionRegistry.Register("trill", new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
            }, _ => true);
        }
    }
}