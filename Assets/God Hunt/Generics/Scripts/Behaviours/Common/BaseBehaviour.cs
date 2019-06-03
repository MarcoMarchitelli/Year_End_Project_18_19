using UnityEngine;

public abstract class BaseBehaviour : MonoBehaviour, IBehaviour
{
    /// <summary>
    /// Riferimento all'entitià che controlla il Behaviour
    /// </summary>
    public IEntity Entity { get; private set; }
    /// <summary>
    /// True se il Behaviour è stato setuppato, false altrimenti
    /// </summary>
    public bool isEnabled { get; private set; }

    /// <summary>
    /// Toggles the activity of the behaviour.
    /// </summary>
    /// <param name="_value">if active or not.</param>
    public virtual void Enable(bool _value)
    {
        isEnabled = _value;
    }

    /// <summary>
    /// Behaviour's custom start.
    /// </summary>
    public virtual void OnStart()
    {

    }

    /// <summary>
    /// Behaviour's custom update.
    /// </summary>
    public virtual void OnFixedUpdate()
    {
        
    }

    /// <summary>
    /// Behaviour's custom fixed update.
    /// </summary>
    public virtual void OnLateUpdate()
    {
        
    }

    /// <summary>
    /// Behaviour's custom late update.
    /// </summary>
    public virtual void OnUpdate()
    {
         
    }

    /// <summary>
    /// Base obligatory setup for every Behaviour.
    /// </summary>
    /// <param name="_entity"></param>
    public void Setup(IEntity _entity)
    {
        Entity = _entity;
        isEnabled = true;
        CustomSetup();
    }

    /// <summary>
    /// Optional setup unique to every Behaviour that implements it.
    /// </summary>
    protected virtual void CustomSetup() { }
}