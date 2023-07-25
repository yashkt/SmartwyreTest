using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter Rebate Indentifier");

        string rebateId = Console.ReadLine();

        Console.WriteLine("Enter Product Identifier");

        string productId = Console.ReadLine();

        Console.WriteLine("Enter volume as a Decimal");

        decimal volume = Decimal.Parse(Console.ReadLine());

        CalculateRebateRequest request = new CalculateRebateRequest();

        request.RebateIdentifier = rebateId;
        request.ProductIdentifier = productId;
        request.Volume = volume;

        RebateService rebateService = new RebateService();

        CalculateRebateResult rebateResult = rebateService.Calculate(request);

        if (rebateResult.Success)
            Console.WriteLine("The operation was successful");
        else
            Console.WriteLine("The operation failed.");
    }
}
