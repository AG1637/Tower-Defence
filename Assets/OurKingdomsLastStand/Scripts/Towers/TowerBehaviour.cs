using System.Drawing;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class TowerBehaviour : MonoBehaviour
{
    public static TowerBehaviour instance;
    [Header("Tower Targetting")]
    public LayerMask enemyLayers;
    public float rotationSpeed = 20;
    private float range = 10; //change size of collider dynamically
    public Transform towerPivot;

    [Header("Bullet Shooting")]
    public Transform bulletSpawn;
    public ProjectilePool bulletPool;
    public float fireRate = 0.1f;
    public float damage = 25f;
    public GameObject shootEffect;
    public AudioClip shootSound;
    public AudioSource audioSource;
    private float cooldown;
    public bool canShoot = false;

    [Header("Upgrade")]
    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    public GameObject upgradeCanvas;
    public Button upgradeButton;
    public TextMeshProUGUI upgradeButtonText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI upgradeCost;
    public TextMeshProUGUI sellAmount;
    public int level = 1;
    public int upgradePrice = 100;
    public int sellPrice = 50;
    public bool maxUpgrade = false;

    public void Initialize(ProjectilePool pool, Transform bulletSpawnTransform, Transform towerPivotTransform = null)
    {
        bulletPool = pool;
        if (bulletSpawnTransform != null) bulletSpawn = bulletSpawnTransform;
        if (towerPivotTransform != null) towerPivot = towerPivotTransform;
    }

    void Start()
    {
        instance = this;
        SphereCollider sc = GetComponent<SphereCollider>();
        range = sc.radius;
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
        upgradeCanvas.transform.LookAt(CameraMovement.instance.playerCamera.transform);
        levelText.text = ("Level " + level.ToString());
        upgradeCost.text = ("( -" + upgradePrice.ToString() + ")");
        sellAmount.text = ("( +" + sellPrice.ToString() + " )");
        SphereCollider sc = GetComponent<SphereCollider>();
        sc.radius = range;
        cooldown -= Time.deltaTime;
        //CHECK TOWER LEVEL
        if (level == 1)
        {
            level1.SetActive(true);
            level2.SetActive(false);
            level3.SetActive(false);
        }
        else if (level == 2)
        {
            level1.SetActive(false);
            level2.SetActive(true);
            level3.SetActive(false);
        }
        else if (level == 3)
        {
            level1.SetActive(false);
            level2.SetActive(false);
            level3.SetActive(true);
            upgradeButtonText.text = ("MAX");
            maxUpgrade = true;
            upgradeButton.interactable = false;
            upgradeCost.text = ("");   
        }
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
        upgradeCanvas.SetActive(true);
    }

    public void Upgrade()
    {
        if (maxUpgrade == true) { return; }
        if (GameManager.instance.coinsRemaining >= upgradePrice)
        {
            GameManager.instance.coinsRemaining -= upgradePrice;
            //Debug.Log("Upgrade");
            level++;
            upgradePrice += 100;
            sellPrice += 100;
            upgradeCanvas.SetActive(false);
            //Upgrade tower stats
            fireRate *= 1.2f;
            damage *= 1.35f;
            range *= 1.3f;
        }
        else 
        {
            TowerPlacement.instance.cannotAffordTowerText.SetActive(true);
            TowerPlacement.instance.StartCoroutine(TowerPlacement.instance.HideText());
        }
    }

    public void Sell()
    {
        //Debug.Log("Sell");
        GameManager.instance.coinsRemaining += sellPrice;
        Destroy(gameObject);
    }

    public void CloseUpgradeCanvas()
    {
        upgradeCanvas.SetActive(false);
    }

}
