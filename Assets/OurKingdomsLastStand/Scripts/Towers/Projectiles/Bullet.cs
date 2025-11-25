using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet instance;

    [Header("Bullet Stats")]
    private float speed = 100f;
    public float lifeTime = 5f; 
    public float damage = 25f;
    float spawnTime;

    [Header("VFX")]
    public GameObject hitEffectPrefab;

    void OnEnable()
    {
        spawnTime = Time.time;
        instance = this;
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
