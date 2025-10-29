using UnitOfWork.App.Services;
using UnitOfWork.Core;

namespace UnitOfWork.App.Operations;

/// <summary>
/// Opération réversible pour débiter un portefeuille utilisateur.
/// </summary>
public sealed class DebitWalletOperation : IReversibleOperation
{
    private readonly WalletService _wallet;
    private readonly string _userId;
    private readonly decimal _amount;

    public DebitWalletOperation(WalletService wallet, string userId, decimal amount)
    {
        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
        _userId = userId ?? throw new ArgumentNullException(nameof(userId));
        _amount = amount;
    }

    /// <summary>
    /// Exécute le débit.
    /// </summary>
    public void Do() => _wallet.Debit(_userId, _amount);

    /// <summary>
    /// Annule le débit en créditant le montant.
    /// </summary>
    public void Undo() => _wallet.Credit(_userId, _amount);
}
