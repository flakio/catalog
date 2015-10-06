using Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using System.Diagnostics;

namespace ProductCatalog.Models
{
    public class ElasticSearchDataHelper
    {
        ElasticClient client;
        int itemCount;

        public ElasticSearchDataHelper(IConfigurationElasticClientSettings elasticClientSettings)
        {
            itemCount = elasticClientSettings.DefaultItemCount;
            client = elasticClientSettings.Client;
        }

        public async Task AddTestData()
        {
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
                Category c = new Category() {
                    Id = j,
                    Name = "Category " + j,
                    Products = allProducts.Where(p => p.CategoryId == j).ToList<Product>() };

                descriptor.Index<Category>(op => op.Document(c));
            }
            var result = await client.BulkAsync(descriptor);
            
        }

    }
}
