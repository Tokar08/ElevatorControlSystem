using System;
using System.Collections.Generic;
using System.Text;

class Tth
{
    public int Id { get; set; }
    public DateTime ArrivalDate { get; set; }
    public string VehicleNumber { get; set; }
    public string ItemOwner { get; set; }
    public string Crop { get; set; }
    public string Reproduction { get; set; }

    public Tth(int id, DateTime arrivalDate, string vehicleNumber, string itemOwner, string crop, string reproduction)
    {
        Id = id;
        ArrivalDate = arrivalDate;
        VehicleNumber = vehicleNumber;
        ItemOwner = itemOwner;
        Crop = crop;
        Reproduction = reproduction;
    }

    public override string ToString()
    {
        return $"Id: {Id}\nArrival date: {ArrivalDate}\nVehicle Number: {VehicleNumber}\nItem owner: {ItemOwner}\nCrop: {Crop}\nReproduction: {Reproduction}\n";
    }
}

class AnalysisCard
{
    public Tth Tth { get; set; }
    public int Id { get; set; }
    public int PhysicalWeight { get; set; }
    public double WeedMoisture { get; set; }
    public double Humidity { get; set; }
    public double? Temperature { get; set; }
    public string Comment { get; set; }

    public AnalysisCard(Tth tth, int id, int physicalWeight, double weedMoisture, double humidity, double? temperature = null, string comment = null)
    {
        Tth = tth;
        Id = id;
        PhysicalWeight = physicalWeight;
        WeedMoisture = weedMoisture;
        Humidity = humidity;
        Temperature = temperature;
        Comment = comment;
    }

    public override string ToString()
    {
        return $"id: {Id}\nPhysical Weight: {PhysicalWeight}\nWeed moisture: {WeedMoisture}\nHumidity: {Humidity}\nTemperature: {Temperature}\nComment: {Comment}";
    }
}

class Ppp
{
    public AnalysisCard Analysis { get; set; }
    public int Id { get; set; }
    public double BaseWeedMoisture { get; set; }
    public double BaseHumidity { get; set; }

    public Ppp(AnalysisCard analysis, int id, double baseWeedMoisture, double baseHumidity)
    {
        Analysis = analysis;
        Id = id;
        BaseHumidity = baseHumidity;
        BaseWeedMoisture = baseWeedMoisture;
    }

    public double ShrinkageCalculation()
    {
        //Данный расчет не имеет ничего общего с формулой был сделан для теста
        return Analysis.PhysicalWeight + Analysis.Humidity + BaseHumidity;
    }

    public double WeedLossCalculation()
    { 
        //Данный расчет не имеет ничего общего с формулой был сделан для теста
        return Analysis.PhysicalWeight + Analysis.Humidity + BaseWeedMoisture;
    }

    public double FinalWeightCalculation()
    {
        //Данный расчет не имеет ничего общего с формулой был сделан для теста
        return Analysis.PhysicalWeight + ShrinkageCalculation() + WeedLossCalculation();
    }
}

class Register
{
    public int Id { get; set; }
    public int RegisterNumber { get; set; }
    public double FinalPhysicalWeight { get; set; }
    public double FinalShrinkage { get; set; }
    public double FinalWeedLoss { get; set; }
    public double FinalAccountWeight { get; set; }

    private List<Ppp> ppps;

    public Register(List<Ppp> ppps, int id, int registerNumber)
    {
        this.ppps = ppps;
        Id = id;
        RegisterNumber = registerNumber;
    }

    public void FinalPppsSummation()
    {
        foreach (Ppp p in ppps)
        {
            FinalPhysicalWeight += p.Analysis.PhysicalWeight;
            FinalShrinkage += p.ShrinkageCalculation();
            FinalWeedLoss += p.WeedLossCalculation();
            FinalAccountWeight += p.FinalWeightCalculation();
        }

        Console.WriteLine(
            "Итоговый физ.вес: " + FinalPhysicalWeight + " " +
            "Итоговая усушка: " + FinalShrinkage + " " +
            "Итоговая сорная убыль: " + FinalWeedLoss + " " +
            "Итоговый зач.вес: " + FinalAccountWeight);
    }
}

class Stock
{
    private List<Register> registers;

    public Stock(List<Register> registers)
    {
        this.registers = registers;
    }

    public void FinalRegistersSummation()
    {
        double totalFinalAccountWeight = 0.0;
        double totalFinalWeedLoss = 0.0;

        foreach (Register register in registers)
        {
            totalFinalAccountWeight += register.FinalAccountWeight;
            totalFinalWeedLoss += register.FinalWeedLoss;
        }

        Console.WriteLine("Общий итоговый зач.вес: " + totalFinalAccountWeight);
        Console.WriteLine("Общая итоговая сорная убыль: " + totalFinalWeedLoss);
    }
}


class Accounting
{
    public string OperationName { get; set; }
    public float NumberOfFinishedProducts { get; set; }
    public float Price { get; set; }
    public float TotalPrice { get; set; }

    public Accounting()
    {

    }

    public Accounting(string operationName, float numberOfFinishedProducts, float price)
    {
        OperationName = operationName;
        NumberOfFinishedProducts = numberOfFinishedProducts;
        Price = price;
    }

    public float GetAllPrice(params Accounting[] operations)
    {
        foreach (var operation in operations)
        {
            OperationName = operation.OperationName;
            TotalPrice += operation.Price;
        }

        Console.WriteLine("\n\n\nСписок операций, за которые была произведена оплата:");
        PrintAllOperations(operations);

        return TotalPrice;
    }

    public void PrintAllOperations(params Accounting[] operations)
    {
        foreach (var operation in operations)
        {
            Console.WriteLine($"Операция: {operation.OperationName}, Цена: {operation.Price}");
        }
    }

    public override string ToString()
    {
        return $"\nTotal price: {TotalPrice}";
    }
}


class Program
{
    public static void Main(string[] args)
    {
        Tth ttn = new Tth(1, DateTime.Now, "AP2132WS", "Vasia", "crop1", "rep");
        Console.WriteLine("TTH:\n\n" + ttn.ToString());

        AnalysisCard analyze = new AnalysisCard(ttn, 1, 200, 50, 12, 36, "My comment");
        Console.WriteLine("\nAnalysis Card:  \n\n" + analyze.ToString() + "\n\n\n");

        Ppp p = new Ppp(analyze, 5, 20, 10);
        Console.WriteLine("Result of ShrinkageCalculation: " + p.ShrinkageCalculation());
        Console.WriteLine("Result of WeedLossCalculation: " + p.WeedLossCalculation());
        Console.WriteLine("Result of FinalWeightCalculation: " + p.FinalWeightCalculation());

        List<Ppp> ppps = new List<Ppp>()
        {
            new Ppp(analyze, 5, 20, 10),
            new Ppp(analyze, 10, 20, 10),
            new Ppp(analyze, 15, 20, 10),
            new Ppp(analyze, 20, 20, 10),
        };

        Register register = new Register(ppps, 1, 2);
        Register register2 = new Register(ppps, 1, 2);
        Register register3 = new Register(ppps, 1, 2);
        Register register4 = new Register(ppps, 1, 2);
        Console.WriteLine();
        Console.WriteLine();
        register.FinalPppsSummation();
        register2.FinalPppsSummation();
        register3.FinalPppsSummation();
        register4.FinalPppsSummation();

        List<Register> registers = new List<Register>
        {
            register,
            register2,
            register3,
            register4,
        };

        Stock stock = new Stock(registers);
        Console.WriteLine();
        Console.WriteLine();
        stock.FinalRegistersSummation();

        Accounting accounting = new Accounting();
        accounting.GetAllPrice
         (
         new Accounting("operation1", 1, 100),
         new Accounting("operation2", 2, 200),
         new Accounting("operation3", 3, 300)
         );

        Console.WriteLine(accounting.ToString());

        Console.ReadLine();
    }
}



