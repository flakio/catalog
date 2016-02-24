using Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using Microsoft.Extensions.Logging;

namespace ProductCatalog.Models
{
    public class ElasticSearchDataHelper
    {
        ElasticClient client;
        int itemCount;

        ILogger log; 
        public ElasticSearchDataHelper(IConfigurationElasticClientSettings elasticClientSettings, ILoggerFactory loggerFactory)
        {
            log = loggerFactory.CreateLogger<ElasticSearchDataHelper>();
            log.LogWarning("Inside ElasticSearchDataHelper");
          
            itemCount = elasticClientSettings.DefaultItemCount;
            client = elasticClientSettings.Client;

            log.LogWarning(DateTime.Now.ToLongTimeString() + "connstring=" + elasticClientSettings.ConnectionString);
        }

        public async Task AddTestData()
        {
            log.LogWarning("Inside AddTest data");
            try
            {

                await Task.Delay(5000);

                for (int i = 0; i < 4; i++)
            {

                IPingResponse x = await client.PingAsync();

                if (x.ConnectionStatus.Success)
                {
                    log.LogWarning(DateTime.Now.ToLongTimeString() + " Connection successful to - " + x.ConnectionStatus.RequestUrl);
                    break;
                }
                else
                {
                    log.LogWarning(DateTime.Now.ToLongTimeString() + " Unable to connect to - " + x.ConnectionStatus.RequestUrl);
                    await Task.Delay(i * 1000);
                }
           

            }

                var allProducts = new List<Product>();
                var descriptor = new BulkDescriptor();
                for (int i = 0; i < itemCount; i++)
                {
                    Product p = new Product() { Id = i, Title = "test " + i, Price = i, CategoryId = 1 };
                    descriptor.Index<Product>(op => op.Document(p));
                    allProducts.Add(p);
                }
                for (int j = 0; j < 10; j++)
                {
                    Category c = new Category()
                    {
                        Id = j,
                        Name = "Category " + j,
                        Products = allProducts.Where(p => p.CategoryId == j).ToList<Product>()
                    };

                    descriptor.Index<Category>(op => op.Document(c));
                }

                log.LogWarning("before bulk async");
                var result = await client.BulkAsync(descriptor);
                log.LogWarning("after bulk async");
            }
            catch (Exception ex)
            {
                log.LogError(DateTime.Now.ToLongTimeString() + " - Ex Caught:" + ex.Message);
              
            }
        }

    }
}
