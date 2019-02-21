using UnityEngine;

public class PlayerEntity : BaseEntity
{
    [SerializeField] bool setupOnStart = false;

    void Start()
    {
        if(setupOnStart)
            SetUpEntity();
    }
}