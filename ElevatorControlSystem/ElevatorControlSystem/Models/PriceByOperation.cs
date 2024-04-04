using System;
using System.Collections.Generic;

namespace GrainElevatorCS_ef.Models;

public partial class PriceByOperation
{
    public int Id { get; set; }

    public string OperationTitle { get; set; } = null!;

    public double OperationPrice { get; set; }

    public int PriceListId { get; set; }

    public virtual PriceList PriceList { get; set; } = null!;

    public PriceByOperation() { }

    public PriceByOperation(string operationTitle, double operationPrice)
    {
        OperationTitle = operationTitle;
        OperationPrice = operationPrice;
    }



}
