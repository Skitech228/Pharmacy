using Microsoft.EntityFrameworkCore;
using Pharmacy.Database;
using Pharmacy.Domain.Entity;
using Pharmacy.Shared.IEntityService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Application.EntityService
{
    public class ProductService : IProductService
    {
        #region Implementation of IService<Product>

        private readonly ApplicationContext _context;

        public ProductService(ApplicationContext context) => _context = context;

        public async Task<bool> AddAsync(Product Product)
        {
            await _context.Products.AddAsync(Product);

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<bool> RemoveAsync(Product Product)
        {
            _context.Products.Remove(Product);

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Product entity)
        {
            _context.Products.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<Product> GetByIdAsync(int id) => await _context.Products.FindAsync(id);

        /// <inheritdoc />
        public async Task<IEnumerable<Product>> GetAllAsync() => await _context.Products.ToListAsync();

        #endregion
    }
}
