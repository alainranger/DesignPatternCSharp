namespace OrderPipelineDemo.Steps;

using OrderPipelineDemo.Models;

public interface IStep
{
    Task<Order> ExecuteAsync(Order order);
}