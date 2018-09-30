using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Czf.ApiWrapper.Kroger;
using Czf.ApiWrapper.Kroger.Requests;
using Czf.ApiWrapper.Kroger.Responses;
using Newtonsoft.Json;
namespace KrogerStoreCompare.Endpoints
{
    public static class ProductSearch
    {
        [FunctionName("ProductSearch")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            KrogerClient krogerClient = Startup.KrogerClient;
            SearchAllRequest  searchAllRequest = JsonConvert.DeserializeObject<SearchAllRequest>(req.Content.ReadAsStringAsync().Result);

            SearchAllResponse searchAllResponse = krogerClient.SearchAll(searchAllRequest);

            ProductsDetailsResponse productsDetailsResponse =  await krogerClient.ProductsDetailsAsync(new ProductsDetailsRequest() { UPCs = searchAllResponse.Upcs });
            
            return req.CreateResponse(HttpStatusCode.OK, 
                new { productsDetailsResponse.Products}
                );
        }
    }
}
