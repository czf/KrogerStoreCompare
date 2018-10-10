using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrogerStoreCompare.Domain
{
    public class StorePrice
    {
        public StoreIdentifier StoreIdentifier { get; set; }
        public decimal? CalculatedReferencePrice { get; set; }
        public decimal? MinimumAdvertisedPrice { get; set; }
        public decimal? ReferencePrice { get; set; }
        public decimal? CalculatedRegularPrice { get; set; }
        public decimal? PriceSale { get; set; }
        public bool HasPrice { get; set; }
        public decimal? PriceNormal { get; set; }
        public decimal? CalculatedPromoPrice { get; set; }
        /// <summary>
        /// Can contain strings like "price $1.29/lb"
        /// </summary>
        public string CustomerFacingSize { get; set; }
    }

}
