using StrategyPattern.Abstractions;

namespace StrategyPattern.Strategies;

public class CreditCardPayment : IPaymentStrategy
{
    public string PaymentType => "CreditCard";

    public decimal Process(decimal amount)
    {
        return amount * 1.02m;
    }
}