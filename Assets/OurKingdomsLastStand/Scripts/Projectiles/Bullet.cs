using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Stats")]
    public float speed = 50f;
    public float lifeTime = 5f; 
    public float damage = 25f;
    float spawnTime;

    [Header("VFX")]
    public GameObject hitEffectPrefab;

    void OnEnable()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        //Move forward in local space
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (Time.time - spawnTime >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
