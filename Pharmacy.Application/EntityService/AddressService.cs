using Microsoft.EntityFrameworkCore;
using Pharmacy.Database;
using Pharmacy.Domain.Entity;
using Pharmacy.Shared.IEntityService;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.EntityService
{
    public class AddressService : IAddressService
    {
        #region Implementation of IService<Address>

        private readonly ApplicationContext _context;

        public AddressService(ApplicationContext context) => _context = context;

        public async Task<bool> AddAsync(Address address)
        {
            await _context.Addresses.AddAsync(address);

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<bool> RemoveAsync(Address address)
        {
            _context.Addresses.Remove(address);

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Address entity)
        {
            _context.Addresses.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<Address> GetByIdAsync(int id) => await _context.Addresses.FindAsync(id);

        /// <inheritdoc />
        public async Task<IEnumerable<Address>> GetAllAsync() => await _context.Addresses.ToListAsync();
        public async Task<Address> GetLastAddressAsync() => await _context.Addresses.LastAsync();

        #endregion
    }
}
