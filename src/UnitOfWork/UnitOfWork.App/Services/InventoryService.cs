namespace UnitOfWork.App.Services;

/// <summary>
/// Service de gestion des stocks de produits.
/// </summary>
public sealed class InventoryService
{
    private readonly Dictionary<string, int> _stock = [];

    /// <summary>
    /// Définit le stock disponible pour un produit.
    /// </summary>
    public void SetStock(string itemId, int quantity)
    {
        if (string.IsNullOrEmpty(itemId))
            throw new ArgumentException("L'ID du produit ne peut pas être vide.", nameof(itemId));

        _stock[itemId] = quantity;
    }

    /// <summary>
    /// Obtient le stock actuel d'un produit.
    /// </summary>
    public int GetStock(string itemId)
    {
        return _stock.TryGetValue(itemId, out var quantity) ? quantity : 0;
    }

    /// <summary>
    /// Réserve une quantité de stock pour un produit.
    /// </summary>
    /// <exception cref="InvalidOperationException">Si le stock est insuffisant.</exception>
    public void Reserve(string sku, int quantity)
    {
        var current = GetStock(sku);
        if (current < quantity)
        {
            throw new InvalidOperationException($"Stock insuffisant pour {sku}. Disponible: {current}, Requis: {quantity}");
        }

        _stock[sku] = current - quantity;
        Console.WriteLine($"[Inventory] Réservation de {quantity} unité(s) de {sku} => Stock restant: {GetStock(sku)}");
    }

    /// <summary>
    /// Libère une quantité de stock précédemment réservée.
    /// </summary>
    public void Release(string sku, int quantity)
    {
        _stock[sku] = GetStock(sku) + quantity;
        Console.WriteLine($"[Inventory] Libération de {quantity} unité(s) de {sku} => Stock restant: {GetStock(sku)}");
    }
}
