using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Czf.ApiWrapper.Kroger;
using Czf.ApiWrapper.Kroger.Domain;
using Czf.ApiWrapper.Kroger.Requests;
using Czf.ApiWrapper.Kroger.Responses;
using Newtonsoft.Json;

using KrogerStoreCompare.Endpoints;
using KrogerStoreCompare.Domain;

namespace KrogerStoreCompare
{
    public static class Startup
    {
        static Startup()
        {
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;

            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductDetail, ProductData>();
                cfg.CreateMap<ProductDetail, StorePricing>();
            }
            


            );

            Mapper = mapperConfiguration.CreateMapper();
            KrogerClient = new KrogerClient();
        }
        public static IMapper Mapper { get; private set; }
        public static KrogerClient KrogerClient { get; private set; }
    }
}
