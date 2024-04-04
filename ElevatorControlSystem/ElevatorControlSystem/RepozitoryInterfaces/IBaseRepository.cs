using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControlSystem.RepozitoryInterfaces
{
    public interface IBaseRepository<T>
    {
        Task<List<T>> GetAllAsync();

        Task CreateAsync(T entity);
        Task<T?> GetAsync(int id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

    }
}
