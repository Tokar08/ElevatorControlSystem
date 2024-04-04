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
    public class InputInvoiceRepository : IInputInvoiceRepository
    {
        private Db db;

        public InputInvoiceRepository(Db db)
        {
            this.db = db;
        }

        public async Task<List<InputInvoice>> GetAllAsync()
        {
            try
            {
                return await db.InputInvoices.Include(s => s.Supplier).Include(d => d.ProductTitle).ToListAsync();

            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task CreateAsync(InputInvoice inv)
        {
            try
            {
                db.InputInvoices.Add(inv);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //TODO
                throw new Exception(ex.Message );

            }
        }

        public async Task<InputInvoice?> GetAsync(int id)
        {
            try
            {
                return await db.InputInvoices.FirstOrDefaultAsync(i => i.Id == id);
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task UpdateAsync(InputInvoice inv)
        {
            try
            {
                db.InputInvoices.Update(inv);
                await db.SaveChangesAsync();

            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }
       
        public async Task DeleteAsync(InputInvoice inv)
        {
            try
            {
                db.InputInvoices.Remove(inv);
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
