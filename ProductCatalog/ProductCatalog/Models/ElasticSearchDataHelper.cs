using Microsoft.Extensions.Logging;
using Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;

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

            log.LogInformation(DateTime.Now.ToLocalTime() + "connstring=" + elasticClientSettings.ConnectionString);
        }

        public async Task AddTestDataInLoop()
        {
            log.LogInformation("Inside AddTest data");
            try
            {

                await Task.Delay(5000);

                for (int i = 0; i < 4; i++)
                {

                    IPingResponse x = await client.PingAsync();
                    

                    if (x.ApiCall.Success)
                    {
                        log.LogInformation(DateTime.Now.ToLocalTime() + " Connection successful to - " + x.ApiCall.Uri);
                        break;
                    }
                    else
                    {
                        log.LogWarning(DateTime.Now.ToLocalTime() + " Unable to connect to - " + x.ApiCall.Uri);
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
                log.LogError(DateTime.Now.ToLocalTime() + " - Ex Caught:" + ex.Message);

            }
        }


        public async Task AddTestData()
        {
            log.LogInformation("Inside AddTest data");
            try
            {

                await Task.Delay(5000);

                for (int i = 0; i < 4; i++)
                {

                    IPingResponse x = await client.PingAsync();

                    if (x.ApiCall.Success)
                    {
                        log.LogInformation(DateTime.Now.ToLocalTime() + " Connection successful to - " + x.ApiCall.Uri);
                        break;
                    }
                    else
                    {
                        log.LogWarning(DateTime.Now.ToLocalTime() + " Unable to connect to - " + x.ApiCall.Uri);
                        await Task.Delay(i * 1000);
                    }


                }

                Category entertainment = new Category()
                {
                    Id = 1,
                    Name = "Entertainment"
                };

                Category equipment = new Category()
                {
                    Id = 2,
                    Name = "Equipment"

                };
                Category foodsupply = new Category()
                {
                    Id = 3,
                    Name = "Food Supply"
                };

                int id = 0;

                var allProducts = new List<Product>();
                allProducts.Add(new Product()
                {
                    Id = id++,
                    Title = "Pet Rock",
                    Quantity = 20,
                    Price = 5.0M,
                    Description = @"Why be lonely when you can have a pet? The Pet Rock is the lowest maintenance pet you'll ever own",
                    ProductArtUrl = @"https://dockerbook.blob.core.windows.net/images/Pet-Bio-Rock-with-terrarium.jpg",
                    CategoryId = entertainment.Id
                });

                allProducts.Add(new Product()
                {
                    Id = id++,
                    Title = "Robo Buddy",
                    Quantity = 1,
                    Price = 399.99M,
                    Description = @"Robo Buddy is the ultimate Robot toy that every child and adult needs!",
                    ProductArtUrl = @"https://dockerbook.blob.core.windows.net/images/ROBO-BUDDY.jpg",
                    CategoryId = entertainment.Id
                });

                allProducts.Add(new Product()
                {
                    Id = id++,
                    Title = "Jet Pack",
                    Quantity = 5,
                    Price = 999.99M,
                    Description = @"Be the envy of your planetary colony with this deluxe hydrogen-powered Jet Pack.",
                    ProductArtUrl = @"https://dockerbook.blob.core.windows.net/images/Jet-Pack.jpg",
                    CategoryId = equipment.Id
                });

                allProducts.Add(new Product()
                {
                    Id = id++,
                    Title = "Moon Boots",
                    Price = 299.99M,
                    Description = @"Hand crafted and heat moldable, these boots will keep you warm when the temperatures hit below 50 degrees!",
                    ProductArtUrl = @"https://dockerbook.blob.core.windows.net/images/Moon-Boot.jpg",
                    CategoryId = equipment.Id

                });

                allProducts.Add(new Product()
                {
                    Id = id++,
                    Title = "Indestructible Flag Pole",
                    Quantity = 300,
                    Price = 75.00M,
                    Description = @"This indescructible, high-suction flag pole helps adventurers claim what is righfully theirs.",
                    ProductArtUrl = @"https://dockerbook.blob.core.windows.net/images/Indestructable-High-Suction-flag-pole.jpg",
                    CategoryId = equipment.Id
                });
                allProducts.Add(new Product()
                {
                    Id = id++,
                    Title = "Emergency Beacon",
                    Quantity = 7,
                    Price = 125.00M,
                    Description = @"This solar powered emergency beacon is a must-have for any adventurer. ",
                    ProductArtUrl = @"https://dockerbook.blob.core.windows.net/images/emergency-beacon.jpg",
                    CategoryId = equipment.Id

                });
                allProducts.Add(new Product()
                {
                    Id = id++,
                    Title = "Short range Lazer blaster",
                    Quantity = 40,
                    Price = 800.00M,
                    Description = @"The best defense is a good offense and the Lazer blaster gives adventurers piece of mind to handle whatever they may encounter.",
                    ProductArtUrl = @"https://dockerbook.blob.core.windows.net/images/LAzer.jpg",
                    CategoryId = equipment.Id
                });
                allProducts.Add(new Product()
                {
                    Id = id++,
                    Title = "Crunch Bar",
                    Quantity = 36,
                    Price = 2.75M,
                    Description = @"Organic, gluten free, and flavor free, Crunch Bars provide the perfect ratio of protein, carbs, and fat for hungry adventurers.",
                    ProductArtUrl = @"https://dockerbook.blob.core.windows.net/images/Crunch-Bar.jpg",
                    CategoryId = foodsupply.Id

                });
                allProducts.Add(new Product()
                {
                    Id = id++,
                    Title = "Hydro Drink",
                    Quantity = 12,
                    Price = 3.50M,
                    Description = @"Hydro Drink packs in double the caffeine of other energy drinks but with potassium and electryolytes to quickly rehydrate.",
                    ProductArtUrl = @"https://dockerbook.blob.core.windows.net/images/Hydo-Drink.jpg",
                    CategoryId = foodsupply.Id

                });

                entertainment.Products = allProducts.Where(c => c.Id == 1).ToList<Product>();
                equipment.Products = allProducts.Where(c => c.Id == 2).ToList<Product>();
                foodsupply.Products = allProducts.Where(c => c.Id == 3).ToList<Product>();

                var descriptor = new BulkDescriptor();

                descriptor.Index<Category>(op => op.Document(entertainment));
                descriptor.Index<Category>(op => op.Document(equipment));
                descriptor.Index<Category>(op => op.Document(foodsupply));

                foreach (var p in allProducts)
                {
                    descriptor.Index<Product>(op => op.Document(p));
                }

                log.LogWarning("before bulk async");
                var result = await client.BulkAsync(descriptor);
                log.LogWarning("after bulk async");
            }
            catch (Exception ex)
            {
                log.LogError(DateTime.Now.ToLocalTime() + " - Ex Caught:" + ex.Message);

            }
        }
    }
}
