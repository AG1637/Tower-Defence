using UnityEngine;
using Unity.AI;
using Unity.Behavior;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats to change for each enemy type")]
    public float maxHealth = 100;
    private float movementSpeed = 15;
    public int damageToEnd = 25; //this the amount of damage that the castle will take if the enemy reaches the end point
    public GameObject deathEffect;
    
    private Vector3 direction = Vector3.right;
    private Vector3 start;
    public float health;
    public float bulletDamage; //the amount of damage that the bullet that hit the enemy does to the enemy
    private bool hasDealtDamage = false; //check if the enemy has already dealt damage to the castle
    void Awake()
    {
        start = transform.position;
        direction = direction.normalized;
        health = maxHealth;
        BehaviorGraphAgent behaviour = GetComponent<BehaviorGraphAgent>(); //gets reference to the speed in the behaviour graph
        behaviour.BlackboardReference.SetVariableValue("Speed", movementSpeed);
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
            GameManager.instance.coinsRemaining += 10;
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
            /*if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }*/
            other.gameObject.SetActive(false); //makes bullet disappear
        }
    }
}
