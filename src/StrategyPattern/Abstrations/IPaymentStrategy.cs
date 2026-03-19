namespace StrategyPattern.Abstractions;

public interface IPaymentStrategy
{
    string PaymentType { get; }
    decimal Process(decimal amount);
}