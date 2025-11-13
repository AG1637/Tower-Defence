using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static List<TowerBehaviour> TowersInGame;
    public bool paused;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI statText;
    [SerializeField] GameObject GameOverScreen;

    [Header("Stats")]
    public int wavesSurvived = 0;
    public int enemiesDefeated = 0;
    public int coinsRemaining = 0;
    public int towersPlaced = 0;

    [Header("Wave Settings")]
    public TextMeshProUGUI timerText;
    public float remainingTime = 180;
    public float textTimer = 2;
    public GameObject nextwaveText;
    private bool showText = false;

    void Start()
    {
        instance = this; //used to reference GameLoopManager from other scripts
        TowersInGame = new List<TowerBehaviour>();
        nextwaveText.SetActive(false);
    }

    private void Update()
    {
        coinText.text = "Coins: " + coinsRemaining;
        
        //Wave timer
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            showText = true;
            wavesSurvived += 1;
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

    public void GameLost()
    {
        statText.text = "Waves Survived: " + wavesSurvived + "\r\nEnemies Defeated: " + enemiesDefeated + "\r\nCoins Remaining: " + coinsRemaining + "\r\nTowers Placed: " + towersPlaced;
        GameOverScreen.SetActive(true);
        Time.timeScale = 0;
        paused = true;
    }
}
