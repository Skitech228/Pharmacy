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
        Task<double> GetAVGPriseAsync(DateTime startDate, DateTime EndDate);
        Task<IEnumerable<Order>> GetOrersOnSelectedDateAsync(DateTime startDate, DateTime EndDate);
    }
}