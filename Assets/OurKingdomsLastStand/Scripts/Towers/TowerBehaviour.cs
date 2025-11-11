using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    [Header("Tower Targetting")]
    public LayerMask enemyLayers;
    public float rotationSpeed = 30;
    public float range = 10f; //change size of collider dynamically
    public Transform towerPivot;

    [Header("Bullet Shooting")]
    public Transform bulletSpawn;
    public ProjectilePool bulletPool;
    public float fireRate = 1f;

    private float cooldown;
    public bool canShoot = false;

    public void Initialize(ProjectilePool pool, Transform bulletSpawnTransform, Transform towerPivotTransform = null)
    {
        bulletPool = pool;
        if (bulletSpawnTransform != null) bulletSpawn = bulletSpawnTransform;
        if (towerPivotTransform != null) towerPivot = towerPivotTransform;
    }

    void Start()
    {
        if (bulletPool == null)
        {
            bulletPool = FindFirstObjectByType<ProjectilePool>();
        }
    }
    void Update()
    {
        var target = FindNearestEnemy();
        if (target != null && canShoot == true)
        {
            Vector3 direction = (target.position - transform.position);
            direction.y = 0f; //keep only horizontal rotation
            if (towerPivot != null)
            {
                var desiredRot = Quaternion.LookRotation(direction);
                towerPivot.rotation = Quaternion.Lerp(towerPivot.rotation, desiredRot, Time.deltaTime * rotationSpeed);
            }

            if (cooldown <= 0f)
            {
                Shoot(target);
                cooldown = 1f / Mathf.Max(0.0001f, fireRate);
            }
        }

        cooldown -= Time.deltaTime;
    }

    Transform FindNearestEnemy()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, range, enemyLayers);
        Transform best = null;
        float bestDistance = float.MaxValue;
        foreach (var c in collisions)
        {
            float distance = Vector3.SqrMagnitude(c.transform.position - transform.position);
            if (distance < bestDistance)
            {
                best = c.transform;
                bestDistance = distance;
            }
        }
        return best;
    }

    void Shoot(Transform target)
    {
            if (bulletPool == null || bulletSpawn == null)
            {
                return;
            }
            var projGO = bulletPool.Spawn(bulletSpawn.position, bulletSpawn.rotation);
            var proj = projGO.GetComponent<Bullet>();
            //play flash/sound here
    }
}
