using System.Drawing;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public LayerMask EnemiesLayer;
    
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

    private void OnTriggerEnter(Collider enemyCollider)
    {
        if (Target == null)
        {
            Target = enemyCollider.transform.parent.gameObject;
        }
        else
        {
            TowerPivot.transform.rotation = Quaternion.LookRotation(enemyCollider.transform.position - transform.position);
        }
    }

    public void Tick()
    {

    }
    void Update()
    {
        Debug.DrawLine(this.transform.position, this.transform.position + this.TowerPivot.transform.forward * 10, UnityEngine.Color.red, 2);

    }

}
