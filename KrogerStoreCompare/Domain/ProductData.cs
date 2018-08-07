using Czf.ApiWrapper.Kroger.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrogerStoreCompare.Domain
{
    public class ProductData : ProductDetail
    {
        public Dictionary<StoreIdentifier, StorePricing> StorePrices { get; set; }
        public ProductData() : base()
        {
            StorePrices = new Dictionary<StoreIdentifier, StorePricing>();
        }
    }
}
