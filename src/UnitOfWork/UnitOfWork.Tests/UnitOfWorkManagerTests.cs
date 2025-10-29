using UnitOfWork.App.Operations;
using UnitOfWork.App.Services;
using UnitOfWork.Core;
using Xunit;

namespace UnitOfWork.Tests;

/// <summary>
/// Tests unitaires pour le UnitOfWorkManager.
/// </summary>
public class UnitOfWorkManagerTests
{
    [Fact]
    public void Commit_WhenAllOperationsSucceed_ShouldExecuteAllOperations()
    {
        // Arrange
        var wallet = new WalletService();
        var inventory = new InventoryService();
        var unitOfWork = new UnitOfWorkManager();

        const string userId = "user-1";
        const string sku = "sku-ABC";
        wallet.SetBalance(userId, 100m);
        inventory.SetStock(sku, 5);

        unitOfWork.Register(new DebitWalletOperation(wallet, userId, 30m));
        unitOfWork.Register(new ReserveInventoryOperation(inventory, sku, 2));

        // Act
        unitOfWork.Commit();

        // Assert
        Assert.Equal(70m, wallet.GetBalance(userId));
        Assert.Equal(3, inventory.GetStock(sku));
    }

    [Fact]
    public void Commit_WhenSecondOperationFails_ShouldRollbackFirstOperation()
    {
        // Arrange
        var wallet = new WalletService();
        var inventory = new InventoryService();
        var unitOfWork = new UnitOfWorkManager();

        const string userId = "user-1";
        const string sku = "sku-ABC";
        wallet.SetBalance(userId, 100m);
        inventory.SetStock(sku, 1);

        // Premier opération : débit de 30 (va réussir)
        unitOfWork.Register(new DebitWalletOperation(wallet, userId, 30m));
        // Deuxième opération : réservation de 5 unités (va échouer - stock insuffisant)
        unitOfWork.Register(new ReserveInventoryOperation(inventory, sku, 5));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => unitOfWork.Commit());

        // Vérification du rollback : le débit doit être annulé
        Assert.Equal(100m, wallet.GetBalance(userId));
        Assert.Equal(1, inventory.GetStock(sku));
    }

    [Fact]
    public void Commit_WhenFirstOperationFails_ShouldNotExecuteSecondOperation()
    {
        // Arrange
        var wallet = new WalletService();
        var inventory = new InventoryService();
        var unitOfWork = new UnitOfWorkManager();

        const string userId = "user-1";
        const string sku = "sku-ABC";
        wallet.SetBalance(userId, 10m); // Solde insuffisant
        inventory.SetStock(sku, 5);

        // Premier opération : débit de 100 (va échouer - fonds insuffisants)
        unitOfWork.Register(new DebitWalletOperation(wallet, userId, 100m));
        // Deuxième opération : ne devrait jamais être exécutée
        unitOfWork.Register(new ReserveInventoryOperation(inventory, sku, 1));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => unitOfWork.Commit());

        // Vérification : aucune modification n'a été faite
        Assert.Equal(10m, wallet.GetBalance(userId));
        Assert.Equal(5, inventory.GetStock(sku)); // Stock inchangé
    }

    [Fact]
    public void Commit_WithMultipleOperations_ShouldExecuteInOrder()
    {
        // Arrange
        var wallet = new WalletService();
        var inventory = new InventoryService();
        var unitOfWork = new UnitOfWorkManager();

        const string userId = "user-1";
        const string sku1 = "sku-ABC";
        const string sku2 = "sku-XYZ";

        wallet.SetBalance(userId, 100m);
        inventory.SetStock(sku1, 5);
        inventory.SetStock(sku2, 10);

        // Enregistrement de 3 opérations
        unitOfWork.Register(new DebitWalletOperation(wallet, userId, 20m));
        unitOfWork.Register(new ReserveInventoryOperation(inventory, sku1, 1));
        unitOfWork.Register(new ReserveInventoryOperation(inventory, sku2, 2));

        // Act
        unitOfWork.Commit();

        // Assert
        Assert.Equal(80m, wallet.GetBalance(userId));
        Assert.Equal(4, inventory.GetStock(sku1));
        Assert.Equal(8, inventory.GetStock(sku2));
    }

    [Fact]
    public void Commit_WhenThirdOperationFails_ShouldRollbackFirstTwoOperations()
    {
        // Arrange
        var wallet = new WalletService();
        var inventory = new InventoryService();
        var unitOfWork = new UnitOfWorkManager();

        const string userId = "user-1";
        const string sku1 = "sku-ABC";
        const string sku2 = "sku-XYZ";

        wallet.SetBalance(userId, 100m);
        inventory.SetStock(sku1, 5);
        inventory.SetStock(sku2, 1);

        // Opération 1 : débit (va réussir)
        unitOfWork.Register(new DebitWalletOperation(wallet, userId, 20m));
        // Opération 2 : réservation sku1 (va réussir)
        unitOfWork.Register(new ReserveInventoryOperation(inventory, sku1, 1));
        // Opération 3 : réservation sku2 (va échouer - stock insuffisant)
        unitOfWork.Register(new ReserveInventoryOperation(inventory, sku2, 5));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => unitOfWork.Commit());

        // Vérification du rollback complet
        Assert.Equal(100m, wallet.GetBalance(userId)); // Débit annulé
        Assert.Equal(5, inventory.GetStock(sku1)); // Réservation annulée
        Assert.Equal(1, inventory.GetStock(sku2)); // Inchangé
    }

    [Fact]
    public void Register_WhenOperationIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var unitOfWork = new UnitOfWorkManager();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => unitOfWork.Register(null!));
    }
}
