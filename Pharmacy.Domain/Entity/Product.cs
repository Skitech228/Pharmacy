using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domain.Entity
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Prise { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public ICollection<Order>? Orders{ get; set; }
    }
}
