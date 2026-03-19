namespace StrategyPattern.Services;

using StrategyPattern.Abstractions;
using StrategyPattern.Strategies;

public class PaymentService
{
    private readonly Dictionary<string, IPaymentStrategy> _strategies;

    public PaymentService(IEnumerable<IPaymentStrategy> strategies)
    {
        _strategies = strategies.ToDictionary(s => s.PaymentType);
    }

    public decimal ProcessPayment(string paymentType, decimal amount)
    {
        if (!_strategies.TryGetValue(paymentType, out var strategy))
            throw new NotSupportedException();

        return strategy.Process(amount);
    }
}