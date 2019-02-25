using UnityEngine;

public class PlatformEntity : BaseEntity
{
    [SerializeField] bool setupOnStart = false;

    void Start()
    {
        if (setupOnStart)
            SetUpEntity();
    }
}