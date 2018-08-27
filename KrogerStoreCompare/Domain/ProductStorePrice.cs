using Czf.ApiWrapper.Kroger.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrogerStoreCompare.Domain
{
    public class ProductStorePrice : ProductDetail
    {
        public List<StorePrice> StorePrices { get; set; }

        public ProductStorePrice()
        {
            StorePrices = new List<StorePrice>();
        }
    }
}
