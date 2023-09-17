using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControlSystem.Models
{
    public class AppDefect
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? CompanyName { get; set; }
        public bool Status { get; set; }
    }
}
