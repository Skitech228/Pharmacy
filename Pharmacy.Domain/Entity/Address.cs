using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domain.Entity
{
    public class Address
    {
        public int AddressId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int House { get; set; }
        public int Flat { get; set; }

    }
}
