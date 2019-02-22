using UnityEngine;

public class TestEntity : BaseEntity
{
    [SerializeField] bool setupOnStart = false;

    void Start()
    {
        if (setupOnStart)
            SetUpEntity();
    }
}