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
    public class ProductionBatchRepository : IProductBatchRepository
    {
        private Db db;

        public ProductionBatchRepository(Db db)
        {
            this.db = db;
        }

        public async Task<List<ProductionBatch>> GetAllAsync()
        {
            try
            {
                return await db.ProductionBatches.Include(pb=>pb.IdNavigation).ToListAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task CreateAsync(ProductionBatch pb)
        {
            try
            {
                db.ProductionBatches.Add(pb);
                await db.SaveChangesAsync();

            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task<ProductionBatch?> GetAsync(int id)
        {
            try
            {
                return await db.ProductionBatches.FirstOrDefaultAsync(pb => pb.Id == id);
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task UpdateAsync(ProductionBatch pb)
        {
            try
            {
                db.ProductionBatches.Update(pb);
                await db.SaveChangesAsync();

            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task DeleteAsync(ProductionBatch pb)
        {
            try
            {
                db.ProductionBatches.Remove(pb);
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
