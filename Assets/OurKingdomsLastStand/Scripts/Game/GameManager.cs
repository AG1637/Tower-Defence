using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static List<TowerBehaviour> TowersInGame;
    public bool paused;
    public bool gameOver = false;
    public bool fastForward = false;
    public bool slowPowerUpActive = false;
    public bool freezePowerUpActive = false;
    public int wavesRemaining = 10;
    public float currentEnemySpeed;
    public float bossTimer = 150; //time the player has to kill the boss
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI wavesText;
    public TextMeshProUGUI loseStatText;
    public TextMeshProUGUI winStatText;
    public TextMeshProUGUI speedUpButtonText;
    [SerializeField] GameObject GameOverScreen;
    [SerializeField] GameObject GameWinScreen;
    [SerializeField] GameObject Tutorial;
    public Button slowPowerUp;
    public Button freezePowerUp;
    public Button speedUp;

    [Header("Stats")]
    public int wavesSurvived = -1; //starts at -1 so that when the first wave is spawned there are 0 waves survived
    public int enemiesDefeated = 0;
    public int coinsRemaining = 300;
    public int towersPlaced = 0;

    void Start()
    {
        Time.timeScale = 0;
        paused = true;
        instance = this; //used to reference GameLoopManager from other scripts
        TowersInGame = new List<TowerBehaviour>();
        Tutorial.SetActive(true);
    }

    private void Update()
    {
        coinText.text = "Coins: " + coinsRemaining + "";
        wavesText.text = "Waves Remaining: " + wavesRemaining;
        if (slowPowerUpActive == true)
        {
            return;
        }
        else
        {
            if (coinsRemaining < 500)
            {
                slowPowerUp.interactable = false;
            }
            else if (coinsRemaining >= 500)
            {
                slowPowerUp.interactable = true;
            }
        }
        if (freezePowerUpActive == true)
        {
            return;
        }
        else
        {
            if (coinsRemaining < 750)
            {
                freezePowerUp.interactable = false;
            }
            else if (coinsRemaining >= 750)
            {
                freezePowerUp.interactable = true;
            }
        }
        if (WaveSpawner.instance.wavesFinishedSpawning == true)
        {
            if (bossTimer > 0)
            {
                bossTimer -= Time.deltaTime;
            }
            else if (bossTimer <= 0 && gameOver == false)
            {
                GameWon();
            }
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
        gameOver = true;
        winStatText.text = "Waves Survived: " + (wavesSurvived - 1) + "\r\nEnemies Defeated: " + enemiesDefeated + "\r\nCoins Remaining: " + coinsRemaining + "\r\nTowers Placed: " + towersPlaced;
        GameWinScreen.SetActive(true);
        Time.timeScale = 0;
        paused = true;
        //LevelTracker.instance.currentLevel += 1;

    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene(2);
    }

    public void TutorialComplete()
    {
        Time.timeScale = 1;
        paused = false;
        fastForward = false;
        Tutorial.SetActive(false);
    }

    public void SpeedUp()
    {
        fastForward = fastForward == true ? false : true;
        if (fastForward == true)
        {
            Time.timeScale = 2;
            speedUpButtonText.text = ">";
        }
        if (fastForward == false)
        {
            Time.timeScale = 1;
            speedUpButtonText.text = ">>";
        }
    }

    public void SlowPowerUp()
    {
        Time.timeScale = 0.5f;
        slowPowerUpActive = true;
        slowPowerUp.interactable = false;
        speedUp.interactable = false;
        coinsRemaining -= 500;
        //Debug.Log("Slow Power-Up Started");
        Invoke("NormalSpeed", 7.5f);
    }

    public void NormalSpeed()
    {
        //Debug.Log("Slow Power-Up Ended");
        Time.timeScale = 1;
        slowPowerUpActive = false;
        slowPowerUp.interactable = true;
        speedUp.interactable = true;
    }

    public void FreezePowerUp()
    {
        currentEnemySpeed = Enemy.instance.movementSpeed;
        Enemy.instance.movementSpeed = 0;
        freezePowerUpActive = true;
        coinsRemaining -= 750;
        freezePowerUp.interactable = false;
        //Debug.Log("Freeze Power-Up Started");
        Invoke("Unfreeze", 10);
    }
    public void Unfreeze()
    {
        Enemy.instance.movementSpeed = currentEnemySpeed;
        freezePowerUpActive = false;
        freezePowerUp.interactable = true;
        //Debug.Log("Freeze Power-Up Ended");
    }
}
