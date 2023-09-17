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
    public class DepotItemRepository : IDepotItemRepository
    {
        private Db db;

        public DepotItemRepository(Db db)
        {
            this.db = db;
        }

        public async Task<List<DepotItem>> GetAllAsync()
        {
            try
            {
                return await db.DepotItems.ToListAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task CreateAsync(DepotItem di)
        {
            try
            {
                db.DepotItems.Add(di);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }
        public async Task<DepotItem?> GetAsync(int id)
        {
            try
            {
                return await db.DepotItems.FirstOrDefaultAsync(di => di.Id == id);
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task UpdateAsync(DepotItem di)
        {
            try
            {
                db.DepotItems.Update(di);
                await db.SaveChangesAsync();

            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task DeleteAsync(DepotItem di)
        {
            try
            {
                db.DepotItems.Remove(di);
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
