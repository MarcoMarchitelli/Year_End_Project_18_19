using UnityEngine;

public class DestroyBehaviour : BaseBehaviour
{
    #region Events
    [SerializeField] UnityVoidEvent OnDestruction;
    #endregion

    /// <summary>
    /// Destroys the gameObject
    /// </summary>
    public void Destroy()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Destroys the gameObject after a given time
    /// </summary>
    /// <param name="_time"></param>
    public void DestroyAfter(float _time)
    {
        Destroy(gameObject, _time);
    }

    private void OnDestroy()
    {
        OnDestruction.Invoke();
    }
}