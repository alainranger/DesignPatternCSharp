using UnitOfWork.App.Services;
using Xunit;

namespace UnitOfWork.Tests;

/// <summary>
/// Tests unitaires pour le service de portefeuille (WalletService).
/// </summary>
public class WalletServiceTests
{
    [Fact]
    public void SetBalance_ShouldUpdateBalance()
    {
        // Arrange
        var wallet = new WalletService();
        const string userId = "user-1";
        const decimal amount = 100m;

        // Act
        wallet.SetBalance(userId, amount);

        // Assert
        Assert.Equal(amount, wallet.GetBalance(userId));
    }

    [Fact]
    public void GetBalance_WhenUserDoesNotExist_ShouldReturnZero()
    {
        // Arrange
        var wallet = new WalletService();

        // Act
        var balance = wallet.GetBalance("non-existent-user");

        // Assert
        Assert.Equal(0m, balance);
    }

    [Fact]
    public void Debit_WithSufficientFunds_ShouldReduceBalance()
    {
        // Arrange
        var wallet = new WalletService();
        const string userId = "user-1";
        wallet.SetBalance(userId, 100m);

        // Act
        wallet.Debit(userId, 30m);

        // Assert
        Assert.Equal(70m, wallet.GetBalance(userId));
    }

    [Fact]
    public void Debit_WithInsufficientFunds_ShouldThrowException()
    {
        // Arrange
        var wallet = new WalletService();
        const string userId = "user-1";
        wallet.SetBalance(userId, 10m);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            wallet.Debit(userId, 50m));

        Assert.Contains("Fonds insuffisants", exception.Message);
        Assert.Equal(10m, wallet.GetBalance(userId)); // Balance unchanged
    }

    [Fact]
    public void Credit_ShouldIncreaseBalance()
    {
        // Arrange
        var wallet = new WalletService();
        const string userId = "user-1";
        wallet.SetBalance(userId, 100m);

        // Act
        wallet.Credit(userId, 50m);

        // Assert
        Assert.Equal(150m, wallet.GetBalance(userId));
    }

    [Fact]
    public void Credit_ForNewUser_ShouldSetBalance()
    {
        // Arrange
        var wallet = new WalletService();
        const string userId = "new-user";

        // Act
        wallet.Credit(userId, 25m);

        // Assert
        Assert.Equal(25m, wallet.GetBalance(userId));
    }

    [Fact]
    public void SetBalance_WithEmptyUserId_ShouldThrowArgumentException()
    {
        // Arrange
        var wallet = new WalletService();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => wallet.SetBalance("", 100m));
        Assert.Throws<ArgumentException>(() => wallet.SetBalance(null!, 100m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10.50)]
    [InlineData(1000)]
    public void Debit_WithVariousAmounts_ShouldWorkCorrectly(decimal amount)
    {
        // Arrange
        var wallet = new WalletService();
        const string userId = "user-1";
        const decimal initialBalance = 2000m;
        wallet.SetBalance(userId, initialBalance);

        // Act
        wallet.Debit(userId, amount);

        // Assert
        Assert.Equal(initialBalance - amount, wallet.GetBalance(userId));
    }

    [Fact]
    public void MultipleOperations_ShouldMaintainCorrectBalance()
    {
        // Arrange
        var wallet = new WalletService();
        const string userId = "user-1";
        wallet.SetBalance(userId, 100m);

        // Act
        wallet.Debit(userId, 20m);    // 100 - 20 = 80
        wallet.Credit(userId, 30m);   // 80 + 30 = 110
        wallet.Debit(userId, 10m);    // 110 - 10 = 100

        // Assert
        Assert.Equal(100m, wallet.GetBalance(userId));
    }
}
