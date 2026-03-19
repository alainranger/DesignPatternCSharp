using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StrategyPattern.Abstractions;
using StrategyPattern.Services;
using StrategyPattern.Strategies;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddScoped<IPaymentStrategy, CreditCardPayment>();
builder.Services.AddScoped<IPaymentStrategy, PayPalPayment>();
builder.Services.AddScoped<IPaymentStrategy, CryptoPayment>();

builder.Services.AddScoped<PaymentService>();

var app = builder.Build();



app.Run();
