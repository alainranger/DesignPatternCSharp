using UnitOfWork.App.Services;
using Xunit;

namespace UnitOfWork.Tests;

/// <summary>
/// Tests unitaires pour le service d'inventaire (InventoryService).
/// </summary>
public class InventoryServiceTests
{
    [Fact]
    public void SetStock_ShouldUpdateStock()
    {
        // Arrange
        var inventory = new InventoryService();
        const string sku = "sku-ABC";
        const int quantity = 10;

        // Act
        inventory.SetStock(sku, quantity);

        // Assert
        Assert.Equal(quantity, inventory.GetStock(sku));
    }

    [Fact]
    public void GetStock_WhenItemDoesNotExist_ShouldReturnZero()
    {
        // Arrange
        var inventory = new InventoryService();

        // Act
        var stock = inventory.GetStock("non-existent-sku");

        // Assert
        Assert.Equal(0, stock);
    }

    [Fact]
    public void Reserve_WithSufficientStock_ShouldReduceStock()
    {
        // Arrange
        var inventory = new InventoryService();
        const string sku = "sku-ABC";
        inventory.SetStock(sku, 10);

        // Act
        inventory.Reserve(sku, 3);

        // Assert
        Assert.Equal(7, inventory.GetStock(sku));
    }

    [Fact]
    public void Reserve_WithInsufficientStock_ShouldThrowException()
    {
        // Arrange
        var inventory = new InventoryService();
        const string sku = "sku-ABC";
        inventory.SetStock(sku, 2);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            inventory.Reserve(sku, 5));

        Assert.Contains("Stock insuffisant", exception.Message);
        Assert.Equal(2, inventory.GetStock(sku)); // Stock unchanged
    }

    [Fact]
    public void Release_ShouldIncreaseStock()
    {
        // Arrange
        var inventory = new InventoryService();
        const string sku = "sku-ABC";
        inventory.SetStock(sku, 10);

        // Act
        inventory.Release(sku, 5);

        // Assert
        Assert.Equal(15, inventory.GetStock(sku));
    }

    [Fact]
    public void Release_ForNewItem_ShouldSetStock()
    {
        // Arrange
        var inventory = new InventoryService();
        const string sku = "new-sku";

        // Act
        inventory.Release(sku, 5);

        // Assert
        Assert.Equal(5, inventory.GetStock(sku));
    }

    [Fact]
    public void SetStock_WithEmptyItemId_ShouldThrowArgumentException()
    {
        // Arrange
        var inventory = new InventoryService();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => inventory.SetStock("", 10));
        Assert.Throws<ArgumentException>(() => inventory.SetStock(null!, 10));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(100)]
    public void Reserve_WithVariousQuantities_ShouldWorkCorrectly(int quantity)
    {
        // Arrange
        var inventory = new InventoryService();
        const string sku = "sku-ABC";
        const int initialStock = 200;
        inventory.SetStock(sku, initialStock);

        // Act
        inventory.Reserve(sku, quantity);

        // Assert
        Assert.Equal(initialStock - quantity, inventory.GetStock(sku));
    }

    [Fact]
    public void Reserve_ExactStock_ShouldLeaveZero()
    {
        // Arrange
        var inventory = new InventoryService();
        const string sku = "sku-ABC";
        inventory.SetStock(sku, 5);

        // Act
        inventory.Reserve(sku, 5);

        // Assert
        Assert.Equal(0, inventory.GetStock(sku));
    }

    [Fact]
    public void MultipleOperations_ShouldMaintainCorrectStock()
    {
        // Arrange
        var inventory = new InventoryService();
        const string sku = "sku-ABC";
        inventory.SetStock(sku, 100);

        // Act
        inventory.Reserve(sku, 20);   // 100 - 20 = 80
        inventory.Release(sku, 10);   // 80 + 10 = 90
        inventory.Reserve(sku, 15);   // 90 - 15 = 75

        // Assert
        Assert.Equal(75, inventory.GetStock(sku));
    }

    [Fact]
    public void MultipleItems_ShouldBeIndependent()
    {
        // Arrange
        var inventory = new InventoryService();
        const string sku1 = "sku-ABC";
        const string sku2 = "sku-XYZ";
        inventory.SetStock(sku1, 10);
        inventory.SetStock(sku2, 20);

        // Act
        inventory.Reserve(sku1, 5);
        inventory.Reserve(sku2, 8);

        // Assert
        Assert.Equal(5, inventory.GetStock(sku1));
        Assert.Equal(12, inventory.GetStock(sku2));
    }
}
