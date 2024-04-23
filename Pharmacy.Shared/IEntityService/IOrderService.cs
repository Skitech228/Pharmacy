using Pharmacy.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pharmacy.Shared.IEntityService
{
    public interface IOrderService : IService<Order>
    {
        Task<double> GetAVGPriseAsync(DateTime startDate, DateTime EndDate, string Condition);
        Task<double> GetMinPriseAsync(DateTime startDate, DateTime EndDate, string Condition);
        Task<double> GetMaxPriseAsync(DateTime startDate, DateTime EndDate, string Condition);
        Task<IEnumerable<Order>> GetOrersOnSelectedDateAsync(DateTime startDate, DateTime EndDate);
    }
}