using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public Transform enemyPrefab;
    public Transform spawnPoint;
    private int waveIndex = 0;
    public float timeBetweenWaves = 10; //time between waves in seconds
    public float textTimer = 2; //how long "next wave" text shows
    private float countdown = 2;
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
            waveIndex++;

            for (int i = 0; i < waveIndex; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f);
            }
        }

        void SpawnEnemy()
        {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }

    }
}
