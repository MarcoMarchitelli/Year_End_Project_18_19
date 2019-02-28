using UnityEngine;

public class SpikeEntity : BaseEntity
{
    [SerializeField] bool setupOnStart = false;

    public virtual void Start()
    {
        if (setupOnStart)
            SetUpEntity();
    }
}