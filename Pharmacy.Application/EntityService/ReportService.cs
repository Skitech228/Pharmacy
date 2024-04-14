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
    public class ReportService : IReportService
    {
        #region Implementation of IService<Report>

        private readonly ApplicationContext _context;

        public ReportService(ApplicationContext context) => _context = context;

        public async Task<bool> AddAsync(Report Report)
        {
            await _context.Reports.AddAsync(Report);

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<bool> RemoveAsync(Report Report)
        {
            _context.Reports.Remove(Report);

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Report entity)
        {
            _context.Reports.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc />
        public async Task<Report> GetByIdAsync(int id) => await _context.Reports.FindAsync(id);

        /// <inheritdoc />
        public async Task<IEnumerable<Report>> GetAllAsync() => await _context.Reports.ToListAsync();

        #endregion
    }
}
