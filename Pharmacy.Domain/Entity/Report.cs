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
    public class Report
    {
        public int ReportId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
    }
}
