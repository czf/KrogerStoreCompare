using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrogerStoreCompare.Domain
{
    public class StoreInfoProductsDetailsRequest
    {
        public StoreIdentifier[] RequestStoreInfos { get; set; }
        public List<string> UPCs { get; set; }
    }
}
