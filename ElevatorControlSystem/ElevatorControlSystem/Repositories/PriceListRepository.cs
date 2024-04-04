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
    public class PriceListRepository : IPriceListRepository
    {
        private Db db;

        public PriceListRepository(Db db)
        {
            this.db = db;
        }

        public async Task<List<PriceList>> GetAllAsync()
        {
            try
            {
                return await db.PriceLists.Include(pl => pl.PriceByOperations).ToListAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task CreateAsync(PriceList pl)
        {
            try
            {
                db.PriceLists.Add(pl);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }


        public async Task<PriceList?> GetAsync(int id)
        {
            try
            {
                return await db.PriceLists.FirstOrDefaultAsync(pl => pl.Id == id);
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task UpdateAsync(PriceList pl)
        {
            try
            {
                db.PriceLists.Update(pl);
                await db.SaveChangesAsync();

            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }
        public async Task DeleteAsync(PriceList pl)
        {
            try
            {
                db.PriceLists.Remove(pl);
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