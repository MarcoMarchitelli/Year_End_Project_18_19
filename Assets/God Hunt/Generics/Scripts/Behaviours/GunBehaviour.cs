using UnityEngine;

public class GunBehaviour : BaseBehaviour
{
    public Projectile projectilePrefab;
    public Transform shootPoint;
    public float timeBetweenEachShot = .5f;
    public float projectileSpeed = 25f;
    public Vector3 projectileRotation;
    public bool startShootingOnAwake = true;

    float timer = 0;
    bool canShoot = true;

    protected override void CustomSetup()
    {
        if (startShootingOnAwake)
        {
            StartShooting();
        }
    }

    private void Update()
    {
        if (canShoot)
            timer -= Time.deltaTime;
        else
        {
            timer += Time.deltaTime;
            if (timer >= timeBetweenEachShot)
                timer = 0;
        }
    }

    public void Shoot()
    {
        if (!canShoot)
            return;

        if (timer <= 0)
        {
            Projectile proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation) as Projectile;
            proj.moveSpeed = projectileSpeed;
            proj.transform.rotation = proj.transform.rotation * Quaternion.Euler(projectileRotation);
            timer = timeBetweenEachShot;
        }
    }

    public void StartShooting()
    {
        canShoot = true;
    }

    public void EndShooting()
    {
        canShoot = false;
    }
}