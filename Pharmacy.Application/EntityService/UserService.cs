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
    public class UserService : IUserService
    {
        #region Implementation of IService<User>

        private readonly ApplicationContext _context;

        public UserService(ApplicationContext context) => _context = context;

        public async Task<bool> AddAsync(User User)
        {
            await _context.Users.AddAsync(User);

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<bool> RemoveAsync(User User)
        {
            _context.Users.Remove(User);

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(User entity)
        {
            _context.Users.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<User> GetByIdAsync(int id) => await _context.Users.FindAsync(id);

        /// <inheritdoc />
        public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.ToListAsync();

        #endregion
    }
}
