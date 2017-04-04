using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Products.Models;
using Microsoft.Extensions.Logging;
using ProductCatalog.Repository;

namespace ProductCatalog.Controllers
{
    [Route("api/[controller]")]
    public class Products : Controller
    {
        private readonly ElasticClient client;
        private readonly int itemcount;
        private ILogger<Products> log;

        public Products(IConfigurationElasticClientSettings elasticClientSettings, ILogger<Products> logger)
        {
            itemcount = elasticClientSettings.DefaultItemCount;
            client = elasticClientSettings.Client;
            log = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            var x = new Class1();
            if (x.IsActive())
            {
                log.LogInformation(x.IsActive().ToString());
            }

            //return top x# based on item count sorted by title
            var result = await client.SearchAsync<Product>(s => s.Size(itemcount));
            return result.Documents;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var searchResults = await client.SearchAsync<Product>(s => s
            .Query(q => q
            .Match(m => m.Field(f => f.Id).Query(id.ToString()))));


            //var t = await client.SearchAsync<Product>(s => s
            //.Query(q => q.Term(t => t.OnField<Product>(p => p.Id))));
            if (searchResults.Documents.Count() == 0)
            {
                return NotFound();
            }
            else if (searchResults.Documents.Count() > 1)
            {
                return new JsonResult(searchResults.Documents.Where(p => p.Id == id).FirstOrDefault());
            }
            else
            {
                return new JsonResult(searchResults.Documents.FirstOrDefault());
            }
        }

        [Route("[action]/{search}")]
        public async Task<IActionResult> Search(string search)
        {
            var searchResults = await client.SearchAsync<Product>(s => s
             .Query(q => q
             .Match(m => m.Field(f => f.Title).Query(search)))
             .Sort(sort => sort.Ascending(f => f.Title)));

            if (searchResults.Documents.Count() == 0)
            {
                return new NotFoundObjectResult(search);
            }
            else
            {
                return new JsonResult(searchResults.Documents);
            }

        }

        [Route("[action]/{id}")]
        public async Task<IActionResult> Category(int id)
        {
            var searchResults = await client.SearchAsync<Product>(s => s
            .Query(q => q
            .Match(m => m.Field(f => f.CategoryId).Query(id.ToString()))));

            if (searchResults.Documents.Count() == 0)
            {
                return NotFound();
            }
            else
            {
                return new JsonResult(searchResults.Documents);
            }
        }

    }
}