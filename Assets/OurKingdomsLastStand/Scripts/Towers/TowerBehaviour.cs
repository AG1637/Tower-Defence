using System.Drawing;
using Unity.AppUI.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class TowerBehaviour : MonoBehaviour
{
    [Header("Tower Targetting")]
    public LayerMask enemyLayers;
    public float rotationSpeed = 20;
    private float range = 10; //change size of collider dynamically
    public Transform towerPivot;

    [Header("Bullet Shooting")]
    public Transform bulletSpawn;
    public ProjectilePool bulletPool;
    public float fireRate = 0.1f;
    public GameObject shootEffect;
    public AudioClip shootSound;
    public AudioSource audioSource;

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
            /*Vector3 dir = target.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(towerPivot.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
            towerPivot.rotation = Quaternion.Euler(0f, rotation.y, 0f);*/
            Vector3 direction = target.transform.position - towerPivot.transform.position;
            Quaternion rotation = Quaternion.Slerp(towerPivot.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            direction.z = 0f;
            towerPivot.transform.rotation = rotation;
            //Debug.DrawLine(towerPivot.transform.position, towerPivot.transform.position + direction * 2f, UnityEngine.Color.red, 0.5f);
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
        if (shootEffect != null)
        {
            Instantiate(shootEffect, transform.position, Quaternion.identity);
        }
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log(name + "Tower Selected");
    }
}
