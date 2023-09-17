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

    public class PriceByOperationRepository : IPriceByOperationRepository
    {
        private Db db;

        public PriceByOperationRepository(Db db)
        {
            this.db = db;
        }

        public async Task<List<PriceByOperation>> GetAllAsync()
        {
            try
            {
                return await db.PriceByOperations.ToListAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task CreateAsync(PriceByOperation po)
        {
            try
            {
                db.PriceByOperations.Add(po);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }


        public async Task<PriceByOperation?> GetAsync(int id)
        {
            try
            {
                return await db.PriceByOperations.FirstOrDefaultAsync(po => po.Id == id);
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task UpdateAsync(PriceByOperation po)
        {
            try
            {
                db.PriceByOperations.Update(po);
                await db.SaveChangesAsync();

            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }
        public async Task DeleteAsync(PriceByOperation po)
        {
            try
            {
                db.PriceByOperations.Remove(po);
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
