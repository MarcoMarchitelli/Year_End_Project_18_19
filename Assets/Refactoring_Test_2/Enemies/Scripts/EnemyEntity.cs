using UnityEngine;

public class EnemyEntity : BaseEntity
{
    [SerializeField] bool setupOnStart = false;

    void Start()
    {
        if (setupOnStart)
            SetUpEntity();
    }
}
