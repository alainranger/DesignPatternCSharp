using OrderPipelineDemo.Models;
using OrderPipelineDemo.Pipelines;
using OrderPipelineDemo.Steps;

var order = new Order(1, 150.00m, "CA");

var pipeline = new OrderPipeline()
    .AddStep(new ValidationStep())
    .AddStep(new TaxCalculationStep())
    .AddStep(new DiscountStep());

var processedOrder = await pipeline.ProcessAsync(order);

Console.WriteLine("---");
Console.WriteLine($"Résultat final pour la commande {processedOrder.Id}:");
Console.WriteLine($"Prix Total: {processedOrder.TotalPrice:C2}");
Console.WriteLine($"Rabais: {string.Join(", ", processedOrder.AppliedDiscounts)}");