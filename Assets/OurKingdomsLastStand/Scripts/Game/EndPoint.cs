using UnityEngine;
using UnityEngine.UI;

public class EndPoint : MonoBehaviour
{
    public int endMaxHealth = 100;
    public int endCurrentHealth;
    public Image healthBarImage;

    private void Awake()
    {
        endCurrentHealth = endMaxHealth;
        healthBarImage.fillAmount = endCurrentHealth / endMaxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(25);
        }
    }

    public void TakeDamage(int amount)
    {
        endCurrentHealth -= amount;
        Debug.Log($"Base took {amount} damage. Remaining health: {endCurrentHealth}");
        healthBarImage.fillAmount = (float)endCurrentHealth / (float)endMaxHealth;

        if (endCurrentHealth <= 0)
        {
            endCurrentHealth = 0;
            GameManager.instance.GameLost();
        }
    }

}
