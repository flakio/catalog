using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;

namespace ProductCatalog
{
    public class ConfigurationElasticClientSettings : IConfigurationElasticClientSettings
    {
        public string ConnectionString { get; }
        public ElasticClient Client { get; }
        public int DefaultItemCount { get; }
        public ConfigurationElasticClientSettings(IConfiguration configuration)
        {


            ConnectionString = (configuration.GetValue<string>("ELASTICSEARCH_PORT")).Replace("tcp", "http");
            Uri uri = new Uri(ConnectionString);

            var elasticConfig = configuration.GetSection("ElasticSearch");
            int count;
            int.TryParse(elasticConfig.GetValue<string>("DefaultItemCount"), out count);
            DefaultItemCount = count;

            ConnectionSettings cs = new ConnectionSettings(uri);
            cs.DefaultIndex(elasticConfig.GetValue<string>("DefaultIndex"));
            Client = new ElasticClient(cs);
        }

    }
}
