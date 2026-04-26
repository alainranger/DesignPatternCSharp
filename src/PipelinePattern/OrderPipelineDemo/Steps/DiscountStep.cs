namespace OrderPipelineDemo.Steps;

using OrderPipelineDemo.Models;

// Étape 3 : Application d'un rabais
public class DiscountStep : IStep
{
    public Task<Order> ExecuteAsync(Order order)
    {
        Console.WriteLine("-> Vérification des rabais...");
        if (order.TotalPrice > 100)
        {
            order.TotalPrice -= 10;
            order.AppliedDiscounts.Add("PROMO10");
        }

        return Task.FromResult(order);
    }
}