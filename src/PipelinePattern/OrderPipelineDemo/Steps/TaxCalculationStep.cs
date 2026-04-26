namespace OrderPipelineDemo.Steps;

using OrderPipelineDemo.Models;

// Étape 2 : Calcul des taxes
public class TaxCalculationStep : IStep
{
    private const decimal CANADA_TAX_RATE = 0.15m;
    private const decimal DEFAULT_TAX_RATE = 0.10m;

    public Task<Order> ExecuteAsync(Order order)
    {
        Console.WriteLine("-> Calcul des taxes...");
        decimal taxRate = order.CountryCode == "CA" ? CANADA_TAX_RATE : DEFAULT_TAX_RATE;
        decimal taxAmount = order.BasePrice * taxRate;
        order.TotalPrice = order.BasePrice + taxAmount;

        return Task.FromResult(order);
    }
}