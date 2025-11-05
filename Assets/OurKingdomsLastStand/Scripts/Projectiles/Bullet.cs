using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Motion")]
    public float speed = 20f;
    public float lifeTime = 5f;

    [Header("Damage")]
    public float damage = 10f;
    public LayerMask hitLayers;

    [Header("VFX")]
    public GameObject hitEffectPrefab;

    float spawnTime;

    void OnEnable()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        //Move forward in local space
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (Time.time - spawnTime >= lifeTime)
            gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        //Ignore collisions with objects not in hitLayers
        if ((hitLayers.value & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }
        //Try to apply damage
        var health = other.GetComponent<Enemy>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        gameObject.SetActive(false);
        Debug.Log("Bullet Hit " + other.gameObject.name);
        health.TakeDamage(damage);
    }
}
