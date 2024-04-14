using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Domain.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public string AccessModifier { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Telephone { get; set; }
        public string Password { get; set; }
        public int AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; }
    }
}
