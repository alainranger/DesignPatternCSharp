namespace OrderPipelineDemo.Models;

public record Order(int Id, decimal BasePrice, string CountryCode)
{
    public decimal TotalPrice { get; set; }
    public List<string> AppliedDiscounts { get; init; } = [];
    public bool IsValid { get; set; } = true;
}