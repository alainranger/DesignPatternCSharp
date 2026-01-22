using FleuntPattern;

var address = new AddressBuilder()
    .SetStreet("123 Main St")
    .SetPostalCode("12345")
    .SetCountry("USA")
    .Build();

Console.WriteLine($"Address: {address.Street}, {address.PostalCode}, {address.Country}");