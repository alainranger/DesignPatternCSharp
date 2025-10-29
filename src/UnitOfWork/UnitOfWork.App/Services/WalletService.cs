namespace UnitOfWork.App.Services;

/// <summary>
/// Service de gestion des portefeuilles utilisateurs.
/// </summary>
public sealed class WalletService
{
    private readonly Dictionary<string, decimal> _balances = [];

    /// <summary>
    /// Définit le solde d'un utilisateur.
    /// </summary>
    public void SetBalance(string userId, decimal amount)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentException("L'ID utilisateur ne peut pas être vide.", nameof(userId));

        _balances[userId] = amount;
    }

    /// <summary>
    /// Obtient le solde actuel d'un utilisateur.
    /// </summary>
    public decimal GetBalance(string userId)
    {
        return _balances.TryGetValue(userId, out var amount) ? amount : 0m;
    }

    /// <summary>
    /// Débite un montant du portefeuille d'un utilisateur.
    /// </summary>
    /// <exception cref="InvalidOperationException">Si les fonds sont insuffisants.</exception>
    public void Debit(string userId, decimal amount)
    {
        var current = GetBalance(userId);
        if (current < amount)
        {
            throw new InvalidOperationException($"Fonds insuffisants pour {userId}. Solde: {current:0.00}, Requis: {amount:0.00}");
        }

        _balances[userId] = current - amount;
        Console.WriteLine($"[Wallet] Débit de {amount:0.00} pour {userId} => Nouveau solde: {GetBalance(userId):0.00}");
    }

    /// <summary>
    /// Crédite un montant au portefeuille d'un utilisateur.
    /// </summary>
    public void Credit(string userId, decimal amount)
    {
        _balances[userId] = GetBalance(userId) + amount;
        Console.WriteLine($"[Wallet] Crédit de {amount:0.00} pour {userId} => Nouveau solde: {GetBalance(userId):0.00}");
    }
}
