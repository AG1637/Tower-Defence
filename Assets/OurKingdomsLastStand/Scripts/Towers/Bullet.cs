using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 25;
    public float lifeTime = 3;
    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            other.GetComponent<Enemy>().health -= damage;
            Destroy(gameObject);
        }
    }
}
