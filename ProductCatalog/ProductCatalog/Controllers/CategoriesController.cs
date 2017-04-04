using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Products.Models;

namespace ProductCatalog.Controllers
{
     [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        
        ElasticClient client;
        private int itemcount;

        public CategoriesController(IConfigurationElasticClientSettings elasticClientSettings)
        {
            client = elasticClientSettings.Client;
            itemcount = elasticClientSettings.DefaultItemCount;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<Category>> Get()
        {
            var result = await client.SearchAsync<Category>(s => s.Size(itemcount));
            return result.Documents;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var searchResults = await client.SearchAsync<Category>(s => s
            .Query(q => q
            .Match(m => m.Field(f => f.Id).Query(id.ToString()))));

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
    }
}