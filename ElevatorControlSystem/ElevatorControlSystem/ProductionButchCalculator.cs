using GrainElevatorCS_ef.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GrainElevatorCS_ef;

public class ProductionButchCalculator
{
    public ProductionBatch ProductionBatch { get; set; }
    private InputInvoice InputInvoice { get; set; }
    private LaboratoryCard LaboratoryCard { get; set; }

    public ProductionButchCalculator(InputInvoice inv, LaboratoryCard lc, ProductionBatch pb)
    {
        InputInvoice = inv;
        LaboratoryCard = lc;
        ProductionBatch = pb;
    }

    public void CalcResultProduction()
    {
        if (LaboratoryCard.Weediness <= ProductionBatch.WeedinessBase)
            ProductionBatch.Waste = 0;
        else
            ProductionBatch.Waste = (int)(InputInvoice.PhysicalWeight * (1 - (100 - LaboratoryCard.Weediness) / (100 - ProductionBatch.WeedinessBase)));

        if (LaboratoryCard.Moisture <= ProductionBatch.MoistureBase)
            ProductionBatch.Shrinkage = 0;
        else
            ProductionBatch.Shrinkage = (int)((InputInvoice.PhysicalWeight - ProductionBatch.Waste) * (1 - (100 - LaboratoryCard.Moisture) / (100 - ProductionBatch.MoistureBase)));

        ProductionBatch.AccountWeight = InputInvoice.PhysicalWeight - ProductionBatch.Waste - ProductionBatch.Shrinkage;
    }


}
