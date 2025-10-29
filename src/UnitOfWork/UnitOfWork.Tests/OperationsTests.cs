using UnitOfWork.App.Operations;
using UnitOfWork.App.Services;
using Xunit;

namespace UnitOfWork.Tests;

/// <summary>
/// Tests unitaires pour les opérations réversibles.
/// </summary>
public class OperationsTests
{
    [Fact]
    public void DebitWalletOperation_Do_ShouldDebitWallet()
    {
        // Arrange
        var wallet = new WalletService();
        const string userId = "user-1";
        wallet.SetBalance(userId, 100m);
        var operation = new DebitWalletOperation(wallet, userId, 30m);

        // Act
        operation.Do();

        // Assert
        Assert.Equal(70m, wallet.GetBalance(userId));
    }

    [Fact]
    public void DebitWalletOperation_Undo_ShouldCreditWallet()
    {
        // Arrange
        var wallet = new WalletService();
        const string userId = "user-1";
        wallet.SetBalance(userId, 100m);
        var operation = new DebitWalletOperation(wallet, userId, 30m);
        operation.Do(); // Débit initial

        // Act
        operation.Undo();

        // Assert
        Assert.Equal(100m, wallet.GetBalance(userId)); // Retour à l'état initial
    }

    [Fact]
    public void DebitWalletOperation_WithNullWallet_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new DebitWalletOperation(null!, "user-1", 10m));
    }

    [Fact]
    public void DebitWalletOperation_WithNullUserId_ShouldThrowArgumentNullException()
    {
        // Arrange
        var wallet = new WalletService();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new DebitWalletOperation(wallet, null!, 10m));
    }

    [Fact]
    public void ReserveInventoryOperation_Do_ShouldReserveStock()
    {
        // Arrange
        var inventory = new InventoryService();
        const string sku = "sku-ABC";
        inventory.SetStock(sku, 10);
        var operation = new ReserveInventoryOperation(inventory, sku, 3);

        // Act
        operation.Do();

        // Assert
        Assert.Equal(7, inventory.GetStock(sku));
    }

    [Fact]
    public void ReserveInventoryOperation_Undo_ShouldReleaseStock()
    {
        // Arrange
        var inventory = new InventoryService();
        const string sku = "sku-ABC";
        inventory.SetStock(sku, 10);
        var operation = new ReserveInventoryOperation(inventory, sku, 3);
        operation.Do(); // Réservation initiale

        // Act
        operation.Undo();

        // Assert
        Assert.Equal(10, inventory.GetStock(sku)); // Retour à l'état initial
    }

    [Fact]
    public void ReserveInventoryOperation_WithNullInventory_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ReserveInventoryOperation(null!, "sku-ABC", 5));
    }

    [Fact]
    public void ReserveInventoryOperation_WithNullSku_ShouldThrowArgumentNullException()
    {
        // Arrange
        var inventory = new InventoryService();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new ReserveInventoryOperation(inventory, null!, 5));
    }

    [Fact]
    public void Operations_DoAndUndo_ShouldBeIdempotent()
    {
        // Arrange
        var wallet = new WalletService();
        var inventory = new InventoryService();
        wallet.SetBalance("user-1", 100m);
        inventory.SetStock("sku-ABC", 10);

        var debitOp = new DebitWalletOperation(wallet, "user-1", 25m);
        var reserveOp = new ReserveInventoryOperation(inventory, "sku-ABC", 3);

        // Act - Do
        debitOp.Do();
        reserveOp.Do();
        var balanceAfterDo = wallet.GetBalance("user-1");
        var stockAfterDo = inventory.GetStock("sku-ABC");

        // Act - Undo
        debitOp.Undo();
        reserveOp.Undo();

        // Assert
        Assert.Equal(75m, balanceAfterDo);
        Assert.Equal(7, stockAfterDo);
        Assert.Equal(100m, wallet.GetBalance("user-1")); // Retour à l'état initial
        Assert.Equal(10, inventory.GetStock("sku-ABC")); // Retour à l'état initial
    }

    [Fact]
    public void Operations_MultipleDoUndo_ShouldWorkCorrectly()
    {
        // Arrange
        var wallet = new WalletService();
        wallet.SetBalance("user-1", 100m);
        var operation = new DebitWalletOperation(wallet, "user-1", 10m);

        // Act & Assert - Premier cycle
        operation.Do();
        Assert.Equal(90m, wallet.GetBalance("user-1"));
        operation.Undo();
        Assert.Equal(100m, wallet.GetBalance("user-1"));

        // Act & Assert - Deuxième cycle
        operation.Do();
        Assert.Equal(90m, wallet.GetBalance("user-1"));
        operation.Undo();
        Assert.Equal(100m, wallet.GetBalance("user-1"));
    }
}
