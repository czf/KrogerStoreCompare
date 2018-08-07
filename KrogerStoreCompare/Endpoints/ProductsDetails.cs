using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Czf.ApiWrapper.Kroger;
using Czf.ApiWrapper.Kroger.Domain;
using Czf.ApiWrapper.Kroger.Requests;
using Czf.ApiWrapper.Kroger.Responses;
using KrogerStoreCompare.Domain;
using Newtonsoft.Json;
namespace KrogerStoreCompare.Endpoints
{
    public static class ProductsDetails
    {
        [FunctionName("ProductsDetails")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            KrogerClient krogerClient = Startup.KrogerClient;

            StoreInfoProductsDetailsRequest storesProductsRequests = JsonConvert.DeserializeObject<StoreInfoProductsDetailsRequest>(req.Content.ReadAsStringAsync().Result);
            Task<ProductsDetailsResponse>[] productsDetailsResponses = new Task<ProductsDetailsResponse>[storesProductsRequests.RequestStoreInfos.Length];
            for (int a = 0; a < storesProductsRequests.RequestStoreInfos.Length; a++)
            {
                StoreIdentifier request = storesProductsRequests.RequestStoreInfos[a];
                productsDetailsResponses[a] =
                krogerClient.ProductsDetailsAsync(
                    new ProductsDetailsRequest()
                    {
                        DivisionId = request.DivisionId,
                        FilterBadProducts = true,
                        StoreId = request.StoreId,
                        UPCs = storesProductsRequests.UPCs
                    });
            }

            Task.WaitAll(productsDetailsResponses);

            Dictionary<string, ProductData> prods = new Dictionary<string, ProductData>();
            for(int a = 0; a < productsDetailsResponses.Length; a++)
            {
                var result = productsDetailsResponses[a].Result;
                foreach(ProductDetail product in result.Products)
                {
                    if (!prods.ContainsKey(product.UPC))
                    {
                        prods[product.UPC] = Startup.Mapper.Map<ProductData>(product);
                        
                    }
                    StoreIdentifier si = new StoreIdentifier()
                    {
                        DivisionId = storesProductsRequests.RequestStoreInfos[a].DivisionId,
                        StoreId = storesProductsRequests.RequestStoreInfos[a].StoreId
                    };
                    prods[product.UPC].StorePrices[si] = Startup.Mapper.Map<StorePricing>(product);
                    prods[product.UPC].StorePrices[si].StoreIdentifier = si;
                }
            }
            
            return req.CreateResponse(HttpStatusCode.OK, prods.Values);
        }

        
    }
    
      

    
    public class RespData
    {
       
    }
}
