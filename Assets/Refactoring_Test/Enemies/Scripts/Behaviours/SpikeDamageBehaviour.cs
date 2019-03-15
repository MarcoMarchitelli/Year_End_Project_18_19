using UnityEngine;
using Refactoring;

public class SpikeDamageBehaviour : BaseBehaviour
{
    [SerializeField] int damage;
    [SerializeField] bool dealsOnTrigger;
    [SerializeField] bool dealsOnCollision;

    private void OnTriggerEnter(Collider other)
    {
        if (!dealsOnTrigger)
            return;

        PlayerEntity p = other.GetComponent<PlayerEntity>();
        if (!p)
            return;

        PlayerEntityData d = p.Data as PlayerEntityData;
        d.damageReceiverBehaviour.SetHealth(-damage);
        d.respawnBehaviour.Respawn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!dealsOnCollision)
            return;

        PlayerEntity p = collision.collider.GetComponent<PlayerEntity>();
        if (!p)
            return;

        PlayerEntityData d = p.Data as PlayerEntityData;
        d.damageReceiverBehaviour.SetHealth(-damage);
        d.respawnBehaviour.Respawn();
    }

}
