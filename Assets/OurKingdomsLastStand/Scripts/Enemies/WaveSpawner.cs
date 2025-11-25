using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.AppUI.Redux;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform spawnPoint;
    private int waveIndex = 0;
    public float timeBetweenWaves = 25; //time between waves in seconds
    public float textTimer = 2; //how long "next wave" text shows
    private float countdown = 11;
    public GameObject nextwaveText;
    public TextMeshProUGUI timerText;
    private bool showText = false;

    public void Start()
    {
        nextwaveText.SetActive(false);
    }

    void Update()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
        }
        else if (countdown <= 0)
        {
            showText = true;
            StartCoroutine(SpawnWave());
            GameManager.instance.wavesSurvived += 1;
            countdown = timeBetweenWaves;
        }
        int minutes = Mathf.FloorToInt(countdown / 60);
        int seconds = Mathf.FloorToInt(countdown % 60);
        timerText.text = string.Format("Next Wave in " + "{0:00}:{1:00}", minutes, seconds);
        if (showText == true)
        {
            if (textTimer > 0)
            {
                textTimer -= Time.deltaTime;
                nextwaveText.SetActive(true);

            }
            else if (textTimer < 0)
            {
                showText = false;
                nextwaveText.SetActive(false);
                textTimer = 2;

            }
        }

        IEnumerator SpawnWave()
        {
            Wave wave = waves[waveIndex];
            for (int z = 0; z < wave.enemies.Length; z++)
            {
                for (int i = 0; i < wave.enemies[z].count; i++)
                {
                    SpawnEnemy(wave.enemies[z].enemy);
                    yield return new WaitForSeconds(wave.spawnRate);
                }
                if (waveIndex == waves.Length)
                {
                    GameManager.instance.GameWon();
                    this.enabled = false;
                }
            }
            waveIndex++;
        }

        void SpawnEnemy(GameObject enemy)
        {
            Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        }
    }
}
