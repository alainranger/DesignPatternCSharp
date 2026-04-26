namespace OrderPipelineDemo.Steps;

using OrderPipelineDemo.Models;

// Étape 1 : Validation
public class ValidationStep : IStep
{
    public Task<Order> ExecuteAsync(Order order)
    {
        Console.WriteLine("-> Validation de la commande...");
        if (order.BasePrice <= 0) order.IsValid = false;
        return Task.FromResult(order);
    }
}