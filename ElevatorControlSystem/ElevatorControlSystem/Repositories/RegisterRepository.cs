using GrainElevatorCS_ef.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControlSystem.Repozitories
{
    public class RegisterRepository : IRegisterRepository
    {
        private Db db;

        public RegisterRepository(Db db)
        {
            this.db = db;
        }

        public async Task<List<Register>> GetAllAsync()
        {
            try
            {
                return await db.Registers.Include(s => s.Supplier).Include(d => d.ProductTitle).ToListAsync();
            }
            catch (Exception)
            {
                // TODO
                throw;
            }
        }

        public async Task CreateAsync(Register register)
        {
            try
            {
                db.Registers.Add(register);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task<Register?> GetAsync(int id)
        {
            try
            {
                return await db.Registers.FirstOrDefaultAsync(r => r.Id == id);
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }


        public async Task UpdateAsync(Register register)
        {
            try
            {
                db.Registers.Update(register);
                await db.SaveChangesAsync();

            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task DeleteAsync(Register register)
        {
            try
            {
                db.Registers.Remove(register);
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
