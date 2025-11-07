using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float maxHealth = 100;
    public float health;
    public GameObject deathEffect;

    [Header("Movement")]
    private Vector3 direction = Vector3.right;
    public float distance = 100;
    public float speed = 25;

    private Vector3 start;
    void Awake()
    {
        start = transform.position;
        direction = direction.normalized;
        health = maxHealth;
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
    }
}
