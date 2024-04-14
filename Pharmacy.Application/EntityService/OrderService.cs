using Microsoft.EntityFrameworkCore;
using Pharmacy.Database;
using Pharmacy.Domain.Entity;
using Pharmacy.Shared.IEntityService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pharmacy.Application.EntityService
{
    public class OrderService : IOrderService
    {
        #region Implementation of IService<Order>

        private readonly ApplicationContext _context;

        public OrderService(ApplicationContext context) => _context = context;

        public async Task<bool> AddAsync(Order Order)
        {
            await _context.Orders.AddAsync(Order);

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<bool> RemoveAsync(Order Order)
        {
            _context.Orders.Remove(Order);

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Order entity)
        {
            _context.Orders.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<Order> GetByIdAsync(int id) => await _context.Orders.FindAsync(id);

        /// <inheritdoc />
        public async Task<IEnumerable<Order>> GetAllAsync() => await _context.Orders.ToListAsync();

        public async Task<double> GetAVGPriseAsync(DateTime startDate, DateTime EndDate) =>
            await _context.Orders.Where(x => x.Date >= startDate && x.Date <= EndDate).Select(x => x.Prise).AverageAsync();

        public async Task<IEnumerable<Order>> GetOrersOnSelectedDateAsync(DateTime startDate, DateTime EndDate) =>
            await _context.Orders.Where(x => x.Date >= startDate).Where(x=>x.Date <= EndDate).ToListAsync();

        #endregion
    }
}
