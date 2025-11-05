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
    public float bulletSpeed = 20f;
    public float bulletDamage = 10f;
    public float fireRate = 1f;

    private float cooldown;

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
        if (target != null)
        {
            Vector3 dir = (target.position - transform.position);
            dir.y = 0f; // keep only horizontal rotation
            if (towerPivot != null)
            {
                var desiredRot = Quaternion.LookRotation(dir);
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
        Collider[] cols = Physics.OverlapSphere(transform.position, range, enemyLayers);
        Transform best = null;
        float bestDist = float.MaxValue;
        foreach (var c in cols)
        {
            float d = Vector3.SqrMagnitude(c.transform.position - transform.position);
            if (d < bestDist)
            {
                best = c.transform;
                bestDist = d;
            }
        }
        return best;
    }

    void Shoot(Transform target)
    {
        if (bulletPool == null || bulletSpawn == null) return;

        var projGO = bulletPool.Spawn(bulletSpawn.position, bulletSpawn.rotation);
        var proj = projGO.GetComponent<Bullet>();
        if (proj != null)
        {
            proj.damage = bulletDamage;
            proj.speed = bulletSpeed;
        }

        //play flash/sound here
    }


    /*
    private void OnTriggerStay(Collider enemyCollider)
    {
        if (target == null) //if no target, find one in range
        {
            target = enemyCollider.transform.parent.gameObject;
            canShoot = false;
        }
        else //if there is a target, rotate towards it then shoot
        {
            Vector3 direction = target.transform.position - towerPivot.transform.position;
            Quaternion rotation = Quaternion.Slerp(towerPivot.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            towerPivot.transform.rotation = rotation;
            Debug.DrawLine(towerPivot.transform.position, towerPivot.transform.position + direction * 2f, UnityEngine.Color.red, 0.5f);
            canShoot = true;
        }
    }
    */

}
