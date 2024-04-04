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
    public class CategoryRepository : ICategoryRepository
    {
        private Db db;

        public CategoryRepository(Db db)
        {
            this.db = db;
        }

        public async Task<List<DepotItemCategory>> GetAllAsync()
        {
            try
            {
                return await db.DepotItemCategories.ToListAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task CreateAsync(DepotItemCategory category)
        {
            try
            {
                db.DepotItemCategories.Add(category);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task<DepotItemCategory?> GetAsync(int id)
        {
            try
            {
                return await db.DepotItemCategories.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task UpdateAsync(DepotItemCategory category)
        {
            try
            {
                db.DepotItemCategories.Update(category);
                await db.SaveChangesAsync();

            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task DeleteAsync(DepotItemCategory category)
        {
            try
            {
                db.DepotItemCategories.Remove(category);
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
