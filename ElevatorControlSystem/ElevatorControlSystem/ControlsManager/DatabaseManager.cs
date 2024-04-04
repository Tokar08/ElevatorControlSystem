using GrainElevatorCS_ef.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ElevatorControlSystem.MainWindow;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace ElevatorControlSystem.ControlsManager
{
    public class DatabaseManager
    {
        private Db dbContext;

        public DatabaseManager(Db context)
        {
            dbContext = context;
        }

        public async Task<(Roles Role, string FirstName, string LastName)> GetUserRoleAsync(string username, string password)
        {
            try
            {
                var user = await dbContext.Users
                    .Where(u => u.Email == username && u.Password == password)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    Roles role = user.Role;
                    string firstName = user.FirstName;
                    string lastName = user.LastName;

                    return (role, firstName, lastName);
                }
                else
                {
                    return (Roles.Def, null, null);
                }
            }
            catch (Exception ex)
            {
                NotificationManager.ShowErrorMessageBox($"Ошибка при проверке учетных данных: {ex.Message}");
                return (Roles.Def, null, null);
            }
        }
    }
}
