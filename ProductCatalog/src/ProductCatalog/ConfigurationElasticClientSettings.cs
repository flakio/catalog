using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ProductCatalog
{
    public class ConfigurationElasticClientSettings : IConfigurationElasticClientSettings
    {
        public string ConnectionString { get; }
        public ElasticClient Client { get;  }
        public int DefaultItemCount { get; }
        public ConfigurationElasticClientSettings(IConfiguration configuration)
        {
     
            ConnectionString = (configuration.Get<string>("ELASTICSEARCH_PORT")).Replace("tcp", "http");
            Uri uri = new Uri(ConnectionString);
           
            var elasticConfig = configuration.GetSection("ElasticSearch");
            int count;
            int.TryParse(elasticConfig.Get<string>("DefaultItemCount"), out count);
            DefaultItemCount = count;
            
            ConnectionSettings cs = new ConnectionSettings(uri, elasticConfig.Get<string>("DefaultIndex"));
            Client = new ElasticClient(cs);

        }

    }
}
