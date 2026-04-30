namespace OrderPipelineDemo.Steps;

using OrderPipelineDemo.Models;

// Étape 3 : Application d'un rabais
public class DiscountStep : IStep
{
    public Task<Order> ExecuteAsync(Order order)
    {
        Console.WriteLine("-> Vérification des rabais...");

        var effectiveTotalPrice = order.TotalPrice > 0 ? order.TotalPrice : order.BasePrice;
        if (effectiveTotalPrice > 100)
        {
            order.TotalPrice = effectiveTotalPrice - 10;
            order.AppliedDiscounts.Add("PROMO10");
        }

        return Task.FromResult(order);
    }
}