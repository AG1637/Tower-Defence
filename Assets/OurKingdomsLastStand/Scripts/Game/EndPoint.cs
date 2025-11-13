using UnityEngine;
using UnityEngine.UI;

public class EndPoint : MonoBehaviour
{
    public int endHealth = 100;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(25);
        }
    }

    public void TakeDamage(int amount)
    {
        endHealth -= amount;
        Debug.Log($"Base took {amount} damage. Remaining health: {endHealth}");

        if (endHealth <= 0)
        {
            endHealth = 0;
            GameManager.instance.GameLost();
        }
    }
}
