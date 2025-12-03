using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [Header("Enemy Stats to change for each enemy type")]
    public float maxHealth = 1000;
    public float movementSpeed = 3;
    public int damageToEnd = 100; //this the amount of damage that the castle will take if the enemy reaches the end point
    public int coins = 500;
    public GameObject deathEffect;

    public Vector3 enemyDirection;
    private Transform target;
    private int wavepointIndex = 0;
    public float health;
    public float bulletDamage; //the amount of damage that the bullet that hit the enemy does to the enemy
    private bool hasDealtDamage = false; //check if the enemy has already dealt damage to the castle
    public Image healthBarFill;
    public GameObject healthBar;

    void Update()
    {
        enemyDirection = target.position - transform.position;
        transform.Translate(enemyDirection.normalized * movementSpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }
        healthBarFill.fillAmount = (float)health / (float)maxHealth;
        healthBar.transform.LookAt(CameraMovement.instance.playerCamera.transform);
    }

    void Start()
    {
        target = Waypoints.points[0];
        health = maxHealth;
        transform.LookAt(target);
        healthBarFill.fillAmount = health / maxHealth;
    }

    void GetNextWaypoint()
    {
        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
        transform.LookAt(target);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }

        void Die()
        {
            if (deathEffect != null) //add death effect when enemy dies - needs adding to the prefab in inspector
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
            //When enemy dies update GameManager stats
            GameManager.instance.enemiesDefeated += 1;
            GameManager.instance.coinsRemaining += 30;
        }
    }

    private void OnTriggerEnter(Collider other) //detect when enemy reaches the end point and destroys itself as well as reducing player health
    {
        if (other.CompareTag("End"))
        {
            if (hasDealtDamage) return; // Prevent multiple damage instances

            EndPoint end = other.GetComponent<EndPoint>();
            if (end != null)
            {
                hasDealtDamage = true;
                end.TakeDamage(damageToEnd);
                Destroy(gameObject);
            }
        }
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(other.GetComponent<Bullet>().damage); //gets reference to bullet script to get damage amount       
            Debug.Log("Hit");
            /*if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }*/
            other.gameObject.SetActive(false); //makes bullet disappear
        }
    }
}
