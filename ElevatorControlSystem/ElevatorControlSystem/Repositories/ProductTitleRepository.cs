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
    public class ProductTitleRepository : IProductTitleRepository
    {
        private Db db;

        public ProductTitleRepository(Db db)
        {
            this.db = db;
        }

        public async Task<List<ProductTitle>> GetAllAsync()
        {
            try
            {
                return await db.ProductTitles.ToListAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task CreateAsync(ProductTitle title)
        {
            try
            {
                db.ProductTitles.Add(title);
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }
        public async Task<ProductTitle?> GetAsync(int id)
        {
            try
            {
                return await db.ProductTitles.FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task UpdateAsync(ProductTitle productTitle)
        {
            try
            {
                db.ProductTitles.Update(productTitle);
                await db.SaveChangesAsync();

            }
            catch (Exception)
            {
                //TODO
                throw;
            }
        }

        public async Task DeleteAsync(ProductTitle title)
        {
            try
            {
                db.ProductTitles.Remove(title);
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
