using ElevatorControlSystem.Models;
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
    public class UserRepository : IUserRepository
    {
        private Db db;

        public UserRepository(Db db)
        {
            this.db = db;

        }

        public async Task<List<User>> GetAllAsync()
        {
            try
            {
                return await db.Users.ToListAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task CreateAsync(User user)
        {
            try
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO

                throw;
            }
        }

        public async Task<User?> GetAsync(int id)
        {
            try
            {
                return await db.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                db.Users.Update(user);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task DeleteAsync(User user)
        {
            try
            {
                db.Users.Remove(user);
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
