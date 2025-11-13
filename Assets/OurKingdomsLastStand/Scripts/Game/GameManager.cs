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
    public int wavesSurvived = -1; //starts at -1 so that when the first wave is spawned there are 0 waves survived
    public int enemiesDefeated = 0;
    public int coinsRemaining = 100;
    public int towersPlaced = 0;

    void Start()
    {
        instance = this; //used to reference GameLoopManager from other scripts
        TowersInGame = new List<TowerBehaviour>();
    }

    private void Update()
    {
        coinText.text = "Coins: " + coinsRemaining; 
    }

    public void GameLost()
    {
        statText.text = "Waves Survived: " + wavesSurvived + "\r\nEnemies Defeated: " + enemiesDefeated + "\r\nCoins Remaining: " + coinsRemaining + "\r\nTowers Placed: " + towersPlaced;
        GameOverScreen.SetActive(true);
        Time.timeScale = 0;
        paused = true;
    }
}
