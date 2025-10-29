namespace UnitOfWork.Core;

/// <summary>
/// Définit un contrat pour gérer un ensemble d'opérations transactionnelles.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Enregistre une opération à exécuter.
    /// </summary>
    void Register(IReversibleOperation operation);

    /// <summary>
    /// Exécute toutes les opérations enregistrées. En cas d'échec, effectue un rollback automatique.
    /// </summary>
    void Commit();

    /// <summary>
    /// Annule manuellement toutes les opérations exécutées.
    /// </summary>
    void Rollback();
}
