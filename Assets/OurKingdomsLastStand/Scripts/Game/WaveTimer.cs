using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float remainingTime = 180;
    public float textTimer = 2;
    public GameObject nextwaveText;
    private bool showText = false;

    private void Start()
    {
        nextwaveText.SetActive(false);
    }

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            showText = true;            
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("Next Wave in " + "{0:00}:{1:00}", minutes, seconds);
        if (showText == true)
        {
            if (textTimer > 0)
            {
                textTimer -= Time.deltaTime;
                remainingTime = 0;
                nextwaveText.SetActive(true);
            }
            else if (textTimer < 0)
            {
                showText = false;
                nextwaveText.SetActive(false);
                remainingTime = 10;
                textTimer = 2;

            }
        }
    }
}
