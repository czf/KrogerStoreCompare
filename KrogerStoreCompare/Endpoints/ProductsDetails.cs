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

            await Task.WhenAll(productsDetailsResponses);

            Dictionary<string, ProductStorePrice> productStorePrices = new Dictionary<string, ProductStorePrice>();
            List<StoreIdentifier> noProductsReturned = new List<StoreIdentifier>();
            for(int a = 0; a< productsDetailsResponses.Length; a++)
            {
                ProductsDetailsResponse result = productsDetailsResponses[a].Result;
                if(result.TotalCount == 0)
                {
                    noProductsReturned.Add(storesProductsRequests.RequestStoreInfos[a]);
                }

                foreach(ProductDetail product in result.Products)
                {
                    if (!productStorePrices.ContainsKey(product.UPC))
                    {
                        productStorePrices[product.UPC] = Startup.Mapper.Map<ProductStorePrice>(product);
                    }
                    productStorePrices[product.UPC].StorePrices.Add(Startup.Mapper.Map<StorePrice>(product));

                    StoreIdentifier si = new StoreIdentifier()
                    {
                        DivisionId = storesProductsRequests.RequestStoreInfos[a].DivisionId,
                        StoreId = storesProductsRequests.RequestStoreInfos[a].StoreId
                    };
                    productStorePrices[product.UPC].StorePrices.Last().StoreIdentifier = si;
                }
            }

            return req.CreateResponse(HttpStatusCode.OK, new { ProductStorePrices = productStorePrices.Values,  NoProductsReturned = noProductsReturned});
        }
    }     
}
