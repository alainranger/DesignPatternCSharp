namespace UnitOfWork.Core;

/// <summary>
/// Représente une opération qui peut être exécutée et annulée (rollback).
/// </summary>
public interface IReversibleOperation
{
    /// <summary>
    /// Exécute l'opération.
    /// </summary>
    void Do();

    /// <summary>
    /// Annule l'opération (rollback).
    /// </summary>
    void Undo();
}
