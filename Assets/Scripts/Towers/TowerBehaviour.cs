using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public Transform TowerPivot;
    public Enemy Target;
    
    public float Damage;
    public float FireRate;
    public float Range;

    private float Delay;

    void Start()
    {
        Delay = 1f / FireRate;
    }

    public void Tick()
    {
        if(Target != null)
        {
            TowerPivot.transform.rotation = Quaternion.LookRotation(Target.transform.position - transform.position);
        }
    }

}
