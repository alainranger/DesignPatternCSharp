namespace StrategyPattern.Strategies;

using StrategyPattern.Abstractions;

public class PayPalPayment : IPaymentStrategy
{
    public string PaymentType => "PayPal";

    public decimal Process(decimal amount)
    {
        return amount * 1.03m;
    }
}