using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.Logging;

namespace ProductCatalog
{
    public class ConfigurationElasticClientSettings : IConfigurationElasticClientSettings
    {
        public string ConnectionString { get; }
        public ElasticClient Client { get;  }
        public int DefaultItemCount { get; }
        public ConfigurationElasticClientSettings(IConfiguration configuration)
        {
            ConnectionString = (configuration.Get("ELASTICSEARCH_PORT")).Replace("tcp", "http");
            Uri uri = new Uri(ConnectionString);

            var elasticConfig = configuration.GetConfigurationSection("ElasticSearch");
            int count;
            int.TryParse(elasticConfig.Get("DefaultItemCount"), out count);
            DefaultItemCount = count;



            
            ConnectionSettings cs = new ConnectionSettings(uri, elasticConfig.Get("DefaultIndex"));
            Client = new ElasticClient(cs);

        }

    }
}
