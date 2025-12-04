using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Stats")]
    private float speed = 100f;
    public float lifeTime = 5f; 
    public float damage;
    float spawnTime;

    [Header("VFX")]
    public GameObject hitEffectPrefab;

    void OnEnable()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        damage = TowerBehaviour.instance.damage;
        //Move forward in local space
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (Time.time - spawnTime >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
