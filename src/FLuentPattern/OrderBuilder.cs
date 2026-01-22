using System.Security.AccessControl;

public class OrderBuilder
{
    private int _number;
    private DateTime _createdAt;
    private readonly AddressBuilder _addressBuilder = AddressBuilder.Empty();

    private OrderBuilder() { }

    public static OrderBuilder Empty() => new();

    public OrderBuilder WithNumber(int number)
    {
        _number = number;
        return this;
    }

    public OrderBuilder CreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public OrderBuilder ShippedTo(Action<AddressBuilder> action)
    {
        action(_addressBuilder);
        return this;
    }

    public Order Build()
    {
        return new Order
        {
            Number = _number,
            CreatedAt = _createdAt,
            ShippingAddress = _addressBuilder.Build()
        };
    }
}
