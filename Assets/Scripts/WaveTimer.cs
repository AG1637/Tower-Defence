using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float remainingTime = 180;
    public float textTimer = 2;
    public GameObject target;
    private bool showText = false;

    private void Start()
    {
        target.SetActive(false);
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
            //Invoke("WaveSpawnerText", 1 * Time.deltaTime);               
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (showText == true)
        {
            if (textTimer > 0)
            {
                textTimer -= Time.deltaTime;
                target.SetActive(true);
            }
            else if (textTimer < 0)
            {
                showText = false;
                target.SetActive(false);
                remainingTime = 10;

            }
        }
    }

    private void WaveSpawnerText()
    {
        target.SetActive(true);
        Invoke("SpawnNextWave", 1 * Time.deltaTime);
        target.SetActive(false);
        remainingTime = 10;
    }
    private void SpawnNextWave()
    {
        Debug.Log("Next Wave");
    }

}
