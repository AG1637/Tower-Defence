using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    private static Queue<int> EnemyIDsToSummon;
    public bool LoopShouldEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyIDsToSummon = new Queue<int>();
        EntitySummoning.Init();

        StartCoroutine(GameLoop());
        InvokeRepeating("SummonTest", 0f, 1f);
    }

    void SummonTest()
    {
        EnqueueEnemyIDToSummon(1);
    }

    IEnumerator GameLoop() //iteration
    {
        while (LoopShouldEnd == false)
        {
            //Spawn Enemies
            if(EnemyIDsToSummon.Count > 0)
            {
                for(int i = 0; i < EnemyIDsToSummon.Count; i++)
                {
                    EntitySummoning.SummonEnemy(EnemyIDsToSummon.Dequeue());
                }
            }

            //Spawn Towers

            yield return null;
        }
    }

    public static void EnqueueEnemyIDToSummon(int ID)
    {
        EnemyIDsToSummon.Enqueue(ID);
    }

}
