using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float speed = 10;

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
