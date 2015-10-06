using Nest;

namespace ProductCatalog
{
    public interface IConfigurationElasticClientSettings
    {
        ElasticClient Client { get; }
        int DefaultItemCount { get; }
    }
}