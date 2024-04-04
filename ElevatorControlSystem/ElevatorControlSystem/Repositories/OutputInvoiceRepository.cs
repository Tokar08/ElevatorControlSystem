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
    public class OutputInvoiceRepository : IOutputInvoiceRepository
    {
        private Db db;

        public OutputInvoiceRepository(Db db)
        {
            this.db = db;
        }

        public async Task<List<OutputInvoice>> GetAllAsync()
        {
            try
            {
                return await db.OutputInvoices.Include(s => s.Supplier).Include(d => d.ProductTitle).ToListAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task CreateAsync(OutputInvoice inv)
        {
            try
            {
                db.OutputInvoices.Add(inv);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task<OutputInvoice?> GetAsync(int id)
        {
            try
            {
                return await db.OutputInvoices.FirstOrDefaultAsync(i => i.Id == id);
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task UpdateAsync(OutputInvoice inv)
        {
            try
            {
                db.OutputInvoices.Update(inv);
                await db.SaveChangesAsync();

            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task DeleteAsync(OutputInvoice inv)
        {
            try
            {
                db.OutputInvoices.Remove(inv);
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