using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interfaccia che definisce il comportamento di un'entità
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Lista di Behaviour dell'entità
    /// </summary>
    List<IBehaviour> Behaviours { get; }

    GameObject gameObject { get; }

    IEntityData Data { get; }

    void SetUpEntity();

    /// <summary>
    /// Entity's custom update.
    /// </summary>
    void OnUpdate();

    /// <summary>
    /// Entity's custom fixed update.
    /// </summary>
    void OnFixedUpdate();

    /// <summary>
    /// Entity's custom late update.
    /// </summary>
    void OnLateUpdate();

    /// <summary>
    /// Searches for the behaviour of type <typeparamref name="T"/> and returns the first one found.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T GetBehaviour<T>() where T : IBehaviour;

    /// <summary>
    /// Searches for all the behaviours of type <typeparamref name="T"/> and returns a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    List<T> GetBehaviours<T>() where T : IBehaviour;
}

/// <summary>
/// Data needed for the entity to work, that comes from outside the entity's behaviours or the entity itself.
/// </summary>
public interface IEntityData
{

}