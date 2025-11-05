using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float maxHealth = 10;
    public float health;
    public GameObject deathEffect;

    [Header("Movement")]
    private Vector3 direction = Vector3.right;
    public float distance = 100;
    public float speed = 25;

    private Vector3 start;

    void Start()
    {
        start = transform.position;
        direction = direction.normalized;
    }

    void Update()
    {
        //temporary movement
        float t = Mathf.PingPong(Time.time * speed, distance);
        transform.position = start + direction * (t - distance * 0.5f);
    }
    void Awake()
    {
        health = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
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

}
