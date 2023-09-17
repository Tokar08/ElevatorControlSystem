using System;
using static ElevatorControlSystem.MainWindow;
namespace ElevatorControlSystem.Models
{
    public class User
    {
        public int id { get; set; }
        public string first_Name { get; set; } = null!;
        public string last_Name { get; set; } = null!;
        public DateTime birth_of_date { get; set; }
        public string email { get; set; } = null!;
        public string phone { get; set; } = null!;
        public string password { get; set; } = null!;
        public string city { get; set; } = null!;
        public string? country { get; set; }
        public Roles role { get; set; }
    }

}
