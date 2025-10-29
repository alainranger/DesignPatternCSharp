namespace UnitOfWork.Core;

/// <summary>
/// Implémentation du pattern Unit of Work avec support des opérations réversibles.
/// Cette classe peut être utilisée dans n'importe quel contexte nécessitant une gestion transactionnelle.
/// </summary>
public sealed class UnitOfWorkManager : IUnitOfWork
{
    private readonly List<IReversibleOperation> _operations = [];
    private readonly Action<string>? _logger;
    private bool _completed;

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="UnitOfWorkManager"/>.
    /// </summary>
    /// <param name="logger">Action optionnelle pour logger les messages (ex: Console.WriteLine).</param>
    public UnitOfWorkManager(Action<string>? logger = null)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public void Register(IReversibleOperation operation)
    {
        if (operation == null)
            throw new ArgumentNullException(nameof(operation));

        _operations.Add(operation);
    }

    /// <inheritdoc />
    public void Commit()
    {
        var executed = new Stack<IReversibleOperation>();

        try
        {
            foreach (var operation in _operations)
            {
                operation.Do();
                executed.Push(operation);
            }
            _completed = true;
        }
        catch
        {
            _logger?.Invoke("[UnitOfWork] Erreur détectée - Début du rollback");

            while (executed.Count > 0)
            {
                var done = executed.Pop();
                try
                {
                    done.Undo();
                }
                catch (Exception undoEx)
                {
                    _logger?.Invoke($"[UnitOfWork] Erreur pendant Undo (ignorée): {undoEx.Message}");
                }
            }

            _completed = true;
            throw;
        }
    }

    /// <inheritdoc />
    public void Rollback()
    {
        if (_completed)
            return;

        for (int i = _operations.Count - 1; i >= 0; i--)
        {
            try
            {
                _operations[i].Undo();
            }
            catch (Exception ex)
            {
                _logger?.Invoke($"[UnitOfWork] Erreur pendant Rollback (ignorée): {ex.Message}");
            }
        }

        _completed = true;
    }
}
