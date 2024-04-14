using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domain.Entity
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Condition { get; set; }
        public double Prise { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
