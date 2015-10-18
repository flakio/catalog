using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using Microsoft.Framework.Configuration;
using System.Diagnostics;

namespace ProductCatalog
{
    public class ConfigurationElasticClientSettings : IConfigurationElasticClientSettings
    {
        public ElasticClient Client { get;  }
        public int DefaultItemCount { get; }
        public ConfigurationElasticClientSettings(IConfiguration configuration)
        {
            int count;
            int.TryParse(configuration.Get("DefaultItemCount"), out count);
            DefaultItemCount = count; 


            Uri uri = new Uri(configuration.Get("ELASTICSEARCH_PORT"));
            
            ConnectionSettings cs = new ConnectionSettings(uri, configuration.Get("DefaultIndex"));
            Client = new ElasticClient(cs);

        }

    }
}
