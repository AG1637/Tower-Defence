using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10;
    public int nodeIndex;
    public float maxHealth = 100;
    public float health;
    public int ID;

    public void Start()
    {
        health = maxHealth;
        transform.position = GameLoopManager.nodePositions[0];
        nodeIndex = 0;
    }
    
    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
