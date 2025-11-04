using System.Drawing;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public LayerMask EnemiesLayer;

    [Header("Tower Variables")]
    public float rotationSpeed = 30;
    public Transform towerPivot;
    public GameObject target;
    public bool canShoot;

    [Header("Bullet Variables")]
    public float bulletSpeed = 150;
    public float damage = 10;
    public float fireRate = 10;
    public float range = 10; //change size of collider dynamically
    private float timer;

    [Header("Initial Setup")]
    public Transform bulletSpawnTransform;
    public GameObject bulletPrefab;

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime / fireRate;
        }
        if (canShoot == true && timer <= 0)
        {
            Shoot();
        }
    }

    private void OnTriggerStay(Collider enemyCollider)
    {
        if (target == null)
        {
            target = enemyCollider.transform.parent.gameObject;
            canShoot = false;
        }
        else
        {
            Vector3 direction = target.transform.position - towerPivot.transform.position;
            Quaternion rotation = Quaternion.Slerp(towerPivot.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            towerPivot.transform.rotation = rotation;
            Debug.DrawLine(towerPivot.transform.position, towerPivot.transform.position + direction * 2f, UnityEngine.Color.red, 0.5f);
            canShoot = true;
        }
    }
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnTransform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("Enemy").transform); //spawns bullet
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnTransform.forward * bulletSpeed, ForceMode.Impulse); //adds force
        timer = 1;
    }

}
