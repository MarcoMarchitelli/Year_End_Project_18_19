/// <summary>
/// Interfaccia che definisce un comportamento
/// </summary>
public interface IBehaviour
{
    /// <summary>
    /// Entità che controlla il Behaviour
    /// </summary>
    IEntity Entity { get; }
    /// <summary>
    /// True se il Behaviour è stato setuppato, false altrimenti
    /// </summary>
    bool isEnabled { get; }

    /// <summary>
    /// Setup del Behaviour
    /// </summary>
    /// <param name="_entity">Riferimento all'entitàche gestisce il Behaviour</param>
    void Setup(IEntity _entity);

    /// <summary>
    /// Behaviour's custom start.
    /// </summary>
    void OnStart();

    /// <summary>
    /// Behaviour's custom update.
    /// </summary>
    void OnUpdate();

    /// <summary>
    /// Behaviour's custom fixed update.
    /// </summary>
    void OnFixedUpdate();

    /// <summary>
    /// Behaviour's custom late update.
    /// </summary>
    void OnLateUpdate();

    /// <summary>
    /// Toggles the activity of the behaviour.
    /// </summary>
    /// <param name="_value">if active or not.</param>
    void Enable(bool _value);
}