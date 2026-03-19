namespace StrategyPattern.Strategies;

using StrategyPattern.Abstractions;

public class CryptoPayment : IPaymentStrategy
{
    public string PaymentType => "Crypto";

    public decimal Process(decimal amount)
    {
        return amount * 0.98m;
    }
}