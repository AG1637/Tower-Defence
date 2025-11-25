using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerBehaviour : MonoBehaviour
{
    [Header("Tower Targetting")]
    public LayerMask enemyLayers;
    public float rotationSpeed = 30;
    private float range = 20; //change size of collider dynamically
    public Transform towerPivot;

    [Header("Bullet Shooting")]
    public Transform bulletSpawn;
    public ProjectilePool bulletPool;
    public float fireRate = 1f;
    public float bulletDamage;

    [Header("TowerUI")]
    public string towerName = "Tower";
    public int level = 1;
    public int upgradeCost;
    public int sellValue;
    public bool archertower;
    public bool magictower; 
    public bool cannontower;

    private float cooldown;
    public bool canShoot = false;
    public float GetRange() => range;

    public void Initialize(ProjectilePool pool, Transform bulletSpawnTransform, Transform towerPivotTransform = null)
    {
        bulletPool = pool;
        if (bulletSpawnTransform != null) bulletSpawn = bulletSpawnTransform;
        if (towerPivotTransform != null) towerPivot = towerPivotTransform;
        bulletDamage = Bullet.instance.damage;
    }

    void Start()
    {
        archertower = TowerPlacement.instance.archer;
        magictower = TowerPlacement.instance.magic;
        cannontower = TowerPlacement.instance.cannon;
        SphereCollider sc = GetComponent<SphereCollider>();
        sc.radius = range;
        if (bulletPool == null)
        {
            bulletPool = FindFirstObjectByType<ProjectilePool>();
        }
        if(GameManager.TowersInGame == null)
        {
            GameManager.TowersInGame.Add(this);
        }
        if (archertower == true)
        {
            archertower = false;
            TowerPlacement.instance.archer = false;
            sellValue = Mathf.RoundToInt(TowerPlacement.instance.archerTowerCost / 4f);
        }
        else if (magictower == true)
        {
            magictower = false;
            TowerPlacement.instance.magic = false;
            sellValue = Mathf.RoundToInt(TowerPlacement.instance.magicTowerCost / 4f);
        }
        else if (cannontower == true)
        {
            cannontower = false;
            TowerPlacement.instance.cannon = false;
            sellValue = Mathf.RoundToInt(TowerPlacement.instance.cannonTowerCost / 4f);
        }
    }
    void OnDestroy()
    {
        if (GameManager.TowersInGame != null)
        {
            GameManager.TowersInGame.Remove(this);
        }
    }

    void Update()
    {
        var target = FindNearestEnemy();
        if (target != null && canShoot == true)
        {
            Vector3 direction = (target.transform.position - transform.position);
            direction.y = 0f; //keep only horizontal rotation
            if (towerPivot != null)
            {
                Vector3 aimPoint = direction + (target.enemyDirection * 100);
                var desiredRot = Quaternion.LookRotation(direction);
                towerPivot.rotation = Quaternion.Lerp(towerPivot.rotation, desiredRot, Time.deltaTime * rotationSpeed);
            }

            if (cooldown <= 0f)
            {
                Shoot();
                cooldown = 1f / Mathf.Max(0.0001f, fireRate);
            }
        }

        cooldown -= Time.deltaTime;
    }

    Enemy FindNearestEnemy()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, range, enemyLayers);
        Enemy best = null;
        float bestDistance = float.MaxValue;
        foreach (var c in collisions)
        {
            float distance = Vector3.SqrMagnitude(c.transform.position - transform.position);
            if (distance < bestDistance)
            {
                best = c.gameObject.GetComponent<Enemy>();
                bestDistance = distance;
            }
        }
        return best;
    }

    void Shoot()
    {
            if (bulletPool == null || bulletSpawn == null)
            {
                return;
            }
            var projGO = bulletPool.Spawn(bulletSpawn.position, bulletSpawn.rotation);
            var proj = projGO.GetComponent<Bullet>();
            //play flash/sound here
    }

    void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) //ignore if user is clicking on UI
        {
            return;
        }
        // Show the tower UI
        if (TowerUIManager.instance != null)
        {
            TowerUIManager.instance.ShowForTower(this);
        }
    }

     public void Upgrade()
    {
        level++;
        // Example stat increases:
        fireRate *= 1.1f;
        range *= 1.05f;
        bulletDamage *= 1.2f;
        Bullet.instance.damage = bulletDamage;
        // increase next upgrade cost and sell value
        upgradeCost = Mathf.RoundToInt(upgradeCost * 1.6f);
        sellValue = Mathf.RoundToInt(sellValue * 1.4f);
        // update collider radius if present
        var sc = GetComponent<SphereCollider>();
        if (sc != null)
        {
            sc.radius = range;
        }
    }

    public void Sell()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.coinsRemaining += sellValue;
        }
        Destroy(gameObject);
    }

}
