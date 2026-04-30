using OrderPipelineDemo.Models;
using OrderPipelineDemo.Steps;

namespace OrderPipelineDemo.Pipelines;

public class OrderPipeline
{
    private readonly List<IStep> _steps = [];

    public OrderPipeline AddStep(IStep step)
    {
        ArgumentNullException.ThrowIfNull(step);
            
        _steps.Add(step);

        return this;
    }

    public async Task<Order> ProcessAsync(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        foreach (var step in _steps)
        {
            order = await step.ExecuteAsync(order);

            if (!order.IsValid)
                break; // Arrêt si une étape invalide la commande
        }

        return order;
    }
}
