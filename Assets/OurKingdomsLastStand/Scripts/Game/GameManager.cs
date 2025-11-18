using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static List<TowerBehaviour> TowersInGame;
    public bool paused;
    public bool gameOver = false;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI loseStatText;
    public TextMeshProUGUI winStatText;
    [SerializeField] GameObject GameOverScreen;
    [SerializeField] GameObject GameWinScreen;

    [Header("Stats")]
    public int wavesSurvived = -1; //starts at -1 so that when the first wave is spawned there are 0 waves survived
    public int enemiesDefeated = 0;
    public int coinsRemaining = 300;
    public int towersPlaced = 0;

    void Start()
    {
        Time.timeScale = 1;
        paused = false;
        instance = this; //used to reference GameLoopManager from other scripts
        TowersInGame = new List<TowerBehaviour>();
    }

    private void Update()
    {
        coinText.text = "Coins: " + coinsRemaining + ""; 
        if (wavesSurvived == 15 && !gameOver)
        {
            GameWon();
            gameOver = true;
        }
    }

    public void GameLost()
    {
        loseStatText.text = "Waves Survived: " + wavesSurvived + "\r\nEnemies Defeated: " + enemiesDefeated + "\r\nCoins Remaining: " + coinsRemaining + "\r\nTowers Placed: " + towersPlaced;
        GameOverScreen.SetActive(true);
        Time.timeScale = 0;
        paused = true;
    }
    public void GameWon()
    {
        winStatText.text = "Waves Survived: " + (wavesSurvived - 1) + "\r\nEnemies Defeated: " + enemiesDefeated + "\r\nCoins Remaining: " + coinsRemaining + "\r\nTowers Placed: " + towersPlaced;
        GameWinScreen.SetActive(true);
        Time.timeScale = 0;
        paused = true;
        LevelTracker.instance.currentLevel += 1;

    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene(2);
    }
}
