using System.Drawing;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public LayerMask EnemiesLayer;

    public float rotationSpeed = 30;
    public Transform TowerPivot;
    public GameObject Target;

    public float Damage = 10;
    public float FireRate = 10;
    public float Range = 10;

    private float Delay;

    void Start()
    {
        Delay = 1f / FireRate;
    }

    private void OnTriggerStay(Collider enemyCollider)
    {
        if (Target == null)
        {
            Target = enemyCollider.transform.parent.gameObject;
        }
        else
        {
            Vector3 direction = Target.transform.position - TowerPivot.transform.position;
            Quaternion rotation = Quaternion.Slerp(TowerPivot.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            TowerPivot.transform.rotation = rotation;
            Debug.DrawLine(TowerPivot.transform.position, TowerPivot.transform.position + direction * 2f, UnityEngine.Color.red, 0.5f);
        }
    }

    public void Tick()
    {

    }
    void Update()
    {
        

    }

}
