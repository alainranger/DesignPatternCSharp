using UnitOfWork.App.Services;
using Xunit;

namespace UnitOfWork.Tests;

/// <summary>
/// Tests unitaires pour le service d'achat avec gestion transactionnelle.
/// </summary>
public class PurchaseServiceTests
{
    [Fact]
    public void Purchase_WhenSufficientFundsAndStock_ShouldCompleteSuccessfully()
    {
        // Arrange
        var wallet = new WalletService();
        var inventory = new InventoryService();
        var purchase = new PurchaseService(wallet, inventory);

        const string userId = "user-1";
        const string sku = "sku-ABC";
        const decimal initialBalance = 100m;
        const int initialStock = 2;
        const int quantityToPurchase = 1;
        const decimal pricePerUnit = 30m;

        wallet.SetBalance(userId, initialBalance);
        inventory.SetStock(sku, initialStock);

        // Act
        purchase.Purchase(userId, sku, quantityToPurchase, pricePerUnit);

        // Assert
        var expectedBalance = initialBalance - (quantityToPurchase * pricePerUnit);
        var expectedStock = initialStock - quantityToPurchase;

        Assert.Equal(expectedBalance, wallet.GetBalance(userId));
        Assert.Equal(expectedStock, inventory.GetStock(sku));
    }

    [Fact]
    public void Purchase_WhenInsufficientStock_ShouldRollbackAndThrowException()
    {
        // Arrange
        var wallet = new WalletService();
        var inventory = new InventoryService();
        var purchase = new PurchaseService(wallet, inventory);

        const string userId = "user-1";
        const string sku = "sku-ABC";
        const decimal initialBalance = 100m;
        const int initialStock = 1;
        const int quantityToPurchase = 5; // Plus que le stock disponible
        const decimal pricePerUnit = 10m;

        wallet.SetBalance(userId, initialBalance);
        inventory.SetStock(sku, initialStock);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            purchase.Purchase(userId, sku, quantityToPurchase, pricePerUnit);
        });

        // Vérification que le message d'erreur contient "Stock insuffisant"
        Assert.Contains("Stock insuffisant", exception.Message);

        // Vérification du rollback : le solde et le stock doivent être inchangés
        Assert.Equal(initialBalance, wallet.GetBalance(userId));
        Assert.Equal(initialStock, inventory.GetStock(sku));
    }

    [Fact]
    public void Purchase_WhenInsufficientFunds_ShouldThrowExceptionImmediately()
    {
        // Arrange
        var wallet = new WalletService();
        var inventory = new InventoryService();
        var purchase = new PurchaseService(wallet, inventory);

        const string userId = "user-1";
        const string sku = "sku-ABC";
        const decimal initialBalance = 10m; // Solde insuffisant
        const int initialStock = 10;
        const int quantityToPurchase = 1;
        const decimal pricePerUnit = 50m; // Plus cher que le solde

        wallet.SetBalance(userId, initialBalance);
        inventory.SetStock(sku, initialStock);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
        {
            purchase.Purchase(userId, sku, quantityToPurchase, pricePerUnit);
        });

        // Vérification que le message d'erreur contient "Fonds insuffisants"
        Assert.Contains("Fonds insuffisants", exception.Message);

        // Vérification : le solde et le stock doivent être inchangés
        Assert.Equal(initialBalance, wallet.GetBalance(userId));
        Assert.Equal(initialStock, inventory.GetStock(sku));
    }

    [Fact]
    public void Purchase_MultiplePurchases_ShouldUpdateBalanceAndStockCorrectly()
    {
        // Arrange
        var wallet = new WalletService();
        var inventory = new InventoryService();
        var purchase = new PurchaseService(wallet, inventory);

        const string userId = "user-1";
        const string sku = "sku-ABC";
        const decimal initialBalance = 100m;
        const int initialStock = 5;

        wallet.SetBalance(userId, initialBalance);
        inventory.SetStock(sku, initialStock);

        // Act - Premier achat
        purchase.Purchase(userId, sku, 1, 20m);

        // Assert - Après premier achat
        Assert.Equal(80m, wallet.GetBalance(userId));
        Assert.Equal(4, inventory.GetStock(sku));

        // Act - Deuxième achat
        purchase.Purchase(userId, sku, 2, 15m);

        // Assert - Après deuxième achat
        Assert.Equal(50m, wallet.GetBalance(userId));
        Assert.Equal(2, inventory.GetStock(sku));
    }

    [Fact]
    public void Purchase_WithZeroQuantity_ShouldCompleteWithoutChanges()
    {
        // Arrange
        var wallet = new WalletService();
        var inventory = new InventoryService();
        var purchase = new PurchaseService(wallet, inventory);

        const string userId = "user-1";
        const string sku = "sku-ABC";
        const decimal initialBalance = 100m;
        const int initialStock = 5;

        wallet.SetBalance(userId, initialBalance);
        inventory.SetStock(sku, initialStock);

        // Act
        purchase.Purchase(userId, sku, 0, 10m);

        // Assert - Aucun changement
        Assert.Equal(initialBalance, wallet.GetBalance(userId));
        Assert.Equal(initialStock, inventory.GetStock(sku));
    }
}
