using UnityEngine;
using Unity.AI;
using Unity.Behavior;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float maxHealth = 100;
    public float health;
    public GameObject deathEffect;
    private float movementSpeed = 15;
    private Vector3 direction = Vector3.right;

    private Vector3 start;
    public float bulletDamage; //the amount of damage that the bullet that hit the enemy does to the enemy
    public float damageToEnd; //this the amount of damage that the castle will take if the enemy reaches the end point
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
            Debug.Log("Enemy Died");
        }
    }

    private void OnTriggerEnter(Collider other) //detect when enemy reaches the end point and destroys itself as well as reducing player health
    {
        if (other.CompareTag("End"))
        {
            //reduce player health
            Debug.Log("Enemy reached the End!");
            Destroy(gameObject);
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
