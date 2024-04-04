using System;
using System.Collections.Generic;

namespace GrainElevatorCS_ef.Models;

public partial class DepotItemCategory
{
    public int Id { get; set; }
    public int DepotItemId { get; set; }
    public string CategoryTitle { get; set; } = null!;
    public int CategoryValue { get; set; }

    public virtual DepotItem DepotItem { get; set; } = null!;

    public DepotItemCategory() { }

    public DepotItemCategory(string categoryTitle, int categoryValue = 0)
    {
        CategoryTitle = categoryTitle;
        CategoryValue = categoryValue;
    }
}
