using UnityEngine;

public abstract class BaseBehaviour : MonoBehaviour, IBehaviour
{
    public System.Action OnBehaviourEnable;
    public System.Action OnBehaviourDisable;

    /// <summary>
    /// Riferimento all'entitià che controlla il Behaviour
    /// </summary>
    public IEntity Entity { get; private set; }
    /// <summary>
    /// True se il Behaviour è stato setuppato, false altrimenti
    /// </summary>
    public bool IsSetupped { get; private set; }

    /// <summary>
    /// Toggles the activity of the behaviour.
    /// </summary>
    /// <param name="_value">if active or not.</param>
    public virtual void Enable(bool _value)
    {
        IsSetupped = _value;
        if (IsSetupped)
            OnBehaviourEnable?.Invoke();
        else
            OnBehaviourDisable?.Invoke();
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
        IsSetupped = true;
        CustomSetup();
    }

    /// <summary>
    /// Optional setup unique to every Behaviour that implements it.
    /// </summary>
    protected virtual void CustomSetup() { }
}