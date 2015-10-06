using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Products.Models;
using Nest;
using ProductCatalog;


namespace Products.Api.Controllers
{
    [Route("api/[controller]")]
    public class Products : Controller
    {
        private readonly ElasticClient client;
        private readonly int itemcount;

        public Products(IConfigurationElasticClientSettings elasticClientSettings)
        {
            itemcount = elasticClientSettings.DefaultItemCount;
            client = elasticClientSettings.Client;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            //return top x# based on item count sorted by title
            var result = await client.SearchAsync<Product>(s => s.Size(itemcount).SortAscending(p => p.Title));
            return result.Documents;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var searchResults = await client.SearchAsync<Product>(s => s
            .Query(q => q
            .Match(m => m.OnField(f => f.Id).Query(id.ToString()))));


            //var t = await client.SearchAsync<Product>(s => s
            //.Query(q => q.Term(t => t.OnField<Product>(p => p.Id))));
            if (searchResults.Documents.Count() == 0)
            {
                return HttpNotFound();
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
             .Match(m => m.OnField(f => f.Title).Query(search)))
             .SortAscending(p=> p.Title));

            if (searchResults.Documents.Count() == 0)
            {
                return new HttpNotFoundResult(); 
            }
            else
            {
                return new JsonResult(searchResults.Documents);
            }       

        }

        [Route("[action]/{id}")]
        public async Task<IActionResult> Category (int id)
        {
            var searchResults = await client.SearchAsync<Product>(s => s
            .Query(q => q
            .Match(m => m.OnField(f => f.CategoryId).Query(id.ToString()))));

            if (searchResults.Documents.Count() == 0)
            {
                return HttpNotFound();
            }
            else
            {
                return new JsonResult(searchResults.Documents);
            }
        }

    }

}
