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
    public class LaboratoryCardRepository : ILaboratoryCardRepository
    {
        private Db db;

        public LaboratoryCardRepository(Db db)
        {
            this.db = db;
        }

        public async Task<List<LaboratoryCard>> GetAllAsync()
        {
            try
            {
                return await db.LaboratoryCards.ToListAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task CreateAsync(LaboratoryCard lc)
        {
            try
            {
                db.LaboratoryCards.Add(lc);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task<LaboratoryCard?> GetAsync(int id)
        {
            try
            {
                return await db.LaboratoryCards.FirstOrDefaultAsync(lc => lc.Id == id);
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task UpdateAsync(LaboratoryCard lc)
        {
            try
            {
                db.LaboratoryCards.Update(lc);
                await db.SaveChangesAsync();

            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task DeleteAsync(LaboratoryCard lc)
        {
            try
            {
                db.LaboratoryCards.Remove(lc);
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