using ElevatorControlSystem.RepozitoryInterfaces;
using GrainElevatorCS_ef.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControlSystem.Repozitories
{
    public class CompletionReportRepository : ICompletionReportRepository
    {
        private Db db;

        public CompletionReportRepository(Db db)
        {
            this.db = db;
        }

        public async Task<List<CompletionReport>> GetAllAsync()
        {
            try
            {
                return await db.CompletionReports.Include(s => s.Supplier).Include(d => d.ProductTitle).Include(cr=> cr.TechnologicalOperations).ToListAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task CreateAsync(CompletionReport report)
        {
            try
            {
                db.CompletionReports.Add(report);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task<CompletionReport?> GetAsync(int id)
        {
            try
            {
                return await db.CompletionReports.FirstOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task UpdateAsync(CompletionReport report)
        {
            try
            {
                db.CompletionReports.Update(report);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task DeleteAsync(CompletionReport report)
        {
            try
            {
                db.CompletionReports.Remove(report);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }




    }
}
