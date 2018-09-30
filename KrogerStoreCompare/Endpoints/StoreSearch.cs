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
    public static class StoreSearch
    {
        [FunctionName("StoreSearch")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "KrogerCompare/StoreSearch/")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");



            KrogerClient client = Startup.KrogerClient;
            StoreSearchRequest storeSearchRequest = JsonConvert.DeserializeObject<StoreSearchRequest>(req.Content.ReadAsStringAsync().Result);

            
            
            StoreSearchResponse storeSearchResponse = client.StoreSearch(storeSearchRequest);
            return req.CreateResponse(HttpStatusCode.OK, storeSearchResponse);
        }
    }
}
