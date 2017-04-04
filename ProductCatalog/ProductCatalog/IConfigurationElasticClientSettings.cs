using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;

namespace ProductCatalog
{
    public interface IConfigurationElasticClientSettings
    {
        ElasticClient Client { get; }
        int DefaultItemCount { get; }
        string ConnectionString { get; }
    }
}
