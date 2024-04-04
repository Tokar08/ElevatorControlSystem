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
    public class TechnologicalOperationRepository : ITechnologicalOperationRepository
    {
        private Db db;

        public TechnologicalOperationRepository(Db db)
        {
            this.db = db;
        }

        public async Task<List<TechnologicalOperation>> GetAllAsync()
        {
            try
            {
                return await db.TechnologicalOperations.Include(to=> to.CompletionReport).ToListAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task CreateAsync(TechnologicalOperation operation)
        {
            try
            {
                db.TechnologicalOperations.Add(operation);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task<TechnologicalOperation?> GetAsync(int id)
        {
            try
            {
                return await db.TechnologicalOperations.FirstOrDefaultAsync(o => o.Id == id);
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task UpdateAsync(TechnologicalOperation operation)
        {
            try
            {
                db.TechnologicalOperations.Update(operation);
                await db.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteAsync(TechnologicalOperation operation)
        {
            try
            {
                db.TechnologicalOperations.Remove(operation);
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
