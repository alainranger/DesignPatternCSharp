namespace OrderPipelineDemo.Steps;

using OrderPipelineDemo.Models;

// Étape 2 : Calcul des taxes
public class TaxCalculationStep : IStep
{
    private const decimal CanadaTaxRate = 0.15m;
    private const decimal DefaultTaxRate = 0.10m;

    public Task<Order> ExecuteAsync(Order order)
    {
        Console.WriteLine("-> Calcul des taxes...");
        decimal taxRate = string.Equals(order.CountryCode, "CA", System.StringComparison.OrdinalIgnoreCase)
            ? CanadaTaxRate
            : DefaultTaxRate;
        decimal taxAmount = order.BasePrice * taxRate;
        order.TotalPrice = order.BasePrice + taxAmount;

        return Task.FromResult(order);
    }
}