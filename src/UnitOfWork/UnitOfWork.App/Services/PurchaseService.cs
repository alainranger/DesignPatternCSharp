using UnitOfWork.App.Operations;
using UnitOfWork.Core;

namespace UnitOfWork.App.Services;

/// <summary>
/// Service coordonnant les achats avec gestion transactionnelle.
/// </summary>
public sealed class PurchaseService
{
    private readonly WalletService _wallet;
    private readonly InventoryService _inventory;

    public PurchaseService(WalletService wallet, InventoryService inventory)
    {
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
        _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
    }

    /// <summary>
    /// Effectue un achat de manière transactionnelle.
    /// Débite le portefeuille et réserve le stock. En cas d'échec, tout est annulé.
    /// </summary>
    public void Purchase(string userId, string sku, int quantity, decimal pricePerUnit)
    {
        var unitOfWork = new UnitOfWorkManager(Console.WriteLine);

        var total = quantity * pricePerUnit;

        // Enregistrement des opérations à effectuer
        unitOfWork.Register(new DebitWalletOperation(_wallet, userId, total));
        unitOfWork.Register(new ReserveInventoryOperation(_inventory, sku, quantity));

        // Exécution transactionnelle (rollback automatique en cas d'erreur)
        unitOfWork.Commit();

        Console.WriteLine($"[Purchase] Transaction complétée avec succès pour {userId}");
    }
}
