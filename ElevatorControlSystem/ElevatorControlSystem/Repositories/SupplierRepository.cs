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
    public class SupplierRepository : ISupplierRepository
    {
        private Db db;

        public SupplierRepository(Db db)
        {
            this.db = db;
        }

        public async Task<List<Supplier>> GetAllAsync()
        {
            try
            {
                return await db.Suppliers.ToListAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task CreateAsync(Supplier supplier)
        {
            try
            {
                db.Suppliers.Add(supplier);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task<Supplier?> GetAsync(int id)
        {
            try
            {
                return await db.Suppliers.FirstOrDefaultAsync(s => s.Id == id);
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            try
            {
                db.Suppliers.Update(supplier);
                await db.SaveChangesAsync();

            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }
        public async Task DeleteAsync(Supplier supplier)
        {
            try
            {
                db.Suppliers.Remove(supplier);
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
