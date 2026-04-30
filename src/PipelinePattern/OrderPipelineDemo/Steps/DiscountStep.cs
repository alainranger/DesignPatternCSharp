namespace OrderPipelineDemo.Steps;

using OrderPipelineDemo.Models;

// Étape 3 : Application d'un rabais
public class DiscountStep : IStep
{
    private const decimal DiscountThreshold = 100m;
    private const decimal DiscountAmount = 10m;

    public Task<Order> ExecuteAsync(Order order)
    {
        Console.WriteLine("-> Vérification des rabais...");

        var effectiveTotalPrice = order.TotalPrice > 0 ? order.TotalPrice : order.BasePrice;
        if (effectiveTotalPrice > DiscountThreshold)
        {
            order.TotalPrice = effectiveTotalPrice - DiscountAmount;
            order.AppliedDiscounts.Add("PROMO10");
        }

        return Task.FromResult(order);
    }
}