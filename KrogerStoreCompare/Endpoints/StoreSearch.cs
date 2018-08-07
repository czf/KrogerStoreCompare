using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Czf.ApiWrapper.Kroger;
using Czf.ApiWrapper.Kroger.Domain;
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

            //// parse query parameter
            //string name = req.GetQueryNameValuePairs()
            //    .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
            //    .Value;

            //if (name == null)
            //{
            //    // Get request body
            //    dynamic data = await req.Content.ReadAsAsync<object>();
            //    name = data?.name;
            //}

            //return name == null
            //    ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
            //    : req.CreateResponse(HttpStatusCode.OK, "Hello " + name);


            KrogerClient client = Startup.KrogerClient;
            StoreSearchRequest storeSearchRequest = JsonConvert.DeserializeObject<StoreSearchRequest>(req.Content.ReadAsStringAsync().Result);

            
            
            StoreSearchResponse storeSearchResponse = client.StoreSearch(storeSearchRequest);
            return req.CreateResponse(HttpStatusCode.OK, storeSearchResponse);
        }
    }
}
