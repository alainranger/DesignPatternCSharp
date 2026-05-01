namespace OrderPipelineDemo.Steps;

using OrderPipelineDemo.Models;

// Étape 1 : Validation
public class ValidationStep : IStep
{
    public Task<Order> ExecuteAsync(Order order)
    {
        Console.WriteLine("-> Validation de la commande...");
        order.IsValid = order.BasePrice > 0;

        return Task.FromResult(order);
    }
}