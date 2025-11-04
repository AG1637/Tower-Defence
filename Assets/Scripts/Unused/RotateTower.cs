using Unity.VisualScripting;
using UnityEngine;

public class RotateTower : MonoBehaviour
{
    public Transform target;
    private float rotationSpeed = 30;

    // Update is called once per frame
    void Update()
    {
        Visuals();
    }

    private void Visuals()
    {
        Vector3 direction = target.transform.position - transform.position;
        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        transform.rotation = rotation;
        Debug.DrawLine(this.transform.position, this.transform.position + direction * 2f, UnityEngine.Color.red, 0.5f);
    }
}
