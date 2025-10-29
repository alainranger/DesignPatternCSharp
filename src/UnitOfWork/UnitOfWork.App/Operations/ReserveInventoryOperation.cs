using UnitOfWork.App.Services;
using UnitOfWork.Core;

namespace UnitOfWork.App.Operations;

/// <summary>
/// Opération réversible pour réserver du stock d'inventaire.
/// </summary>
public sealed class ReserveInventoryOperation : IReversibleOperation
{
    private readonly InventoryService _inventory;
    private readonly string _sku;
    private readonly int _quantity;

    public ReserveInventoryOperation(InventoryService inventory, string sku, int quantity)
    {
        _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
        _sku = sku ?? throw new ArgumentNullException(nameof(sku));
        _quantity = quantity;
    }

    /// <summary>
    /// Exécute la réservation de stock.
    /// </summary>
    public void Do() => _inventory.Reserve(_sku, _quantity);

    /// <summary>
    /// Annule la réservation en libérant le stock.
    /// </summary>
    public void Undo() => _inventory.Release(_sku, _quantity);
}
