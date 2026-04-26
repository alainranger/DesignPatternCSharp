namespace OrderPipelineDemo.Steps;

using OrderPipelineDemo.Models;

// Étape 2 : Calcul des taxes (Ex: Québec 14.975%)
public class TaxCalculationStep : IStep
{
    public Task<Order> ExecuteAsync(Order order)
    {
        Console.WriteLine("-> Calcul des taxes...");
        decimal taxRate = order.CountryCode == "CA" ? 1.15m : 1.10m;
        order.TotalPrice = order.BasePrice * taxRate;
        return Task.FromResult(order);
    }
}