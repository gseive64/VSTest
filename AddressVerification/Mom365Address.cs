using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AddressVerification
{
    class Mom365Address
    {
        public Mom365Address(string line)
        {
            // TODO: Complete member initialization
            var dataElements = line.Split(',');
            this.customerNumber = dataElements[0];
            this.orderKey = dataElements[1];
            this.addressNumber = dataElements[2];
            this.address1 = dataElements[3];
            this.address2 = dataElements[4];
            this.city = dataElements[5];
            this.state = dataElements[6];
            this.zip = dataElements[7];
            this.country = dataElements[8];
            this.cdyneReturnCode = 0;
        }

        public string orderKey { get; private set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string zip { get; set; }

        public int cdyneReturnCode { get; set; }

        public string customerNumber { get; set; }

        public string addressNumber { get; set; }

        public string country { get; set; }
    }
}
