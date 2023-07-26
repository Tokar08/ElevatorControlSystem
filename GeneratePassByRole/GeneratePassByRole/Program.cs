using System;

public class Program
{

    public static string GeneratePass(string postfix)
    {
        const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
       
        char[] password = new char[8];

        for (int i = 0; i < 7; i++)
        {
            password[i] = allowedChars[new Random().Next(allowedChars.Length)];
        }

        return new string(password) + postfix;
    }

    public static void Main()
    {
        Console.WriteLine(GeneratePass("_Bss"));
        Console.WriteLine(GeneratePass("_Bss"));
        Console.WriteLine(GeneratePass("_Bss"));

        Console.WriteLine();


        Console.WriteLine(GeneratePass("_Adm"));
        Console.WriteLine(GeneratePass("_Adm"));

        Console.ReadLine();
    }
}
