using GrainElevatorCS_ef.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorControlSystem.Models
{
    public partial class Category
    {
        public int Id { get; set; }
        public int DepotItemId { get; set; }
        public string CategoryTitle { get; set; } = null!;
        public int CategoryValue { get; set; }

        public virtual DepotItem DepotItem { get; set; } = null!;

        public Category() { }

        public Category(string categoryTitle, int categoryValue = 0)
        {
            CategoryTitle = categoryTitle;
            CategoryValue = categoryValue;
        }
    }

}
