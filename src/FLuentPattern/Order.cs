public class Order
{
    public int Number { get; init; }
    public DateTime CreatedAt { get; init; }
    public required Address ShippingAddress { get; set; }
}

public class Address
{
    public required string Street { get; init; }
    public required string City { get; init; }
    public required string PostalCode { get; init; }
    public required string Country { get; init; }
}