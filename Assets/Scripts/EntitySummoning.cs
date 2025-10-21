using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EntitySummoning : MonoBehaviour
{
    public static List<Enemy> EnemiesInGame;
    public static Dictionary<int, GameObject> EnemyPrefabs;
    public static Dictionary<int, Queue<Enemy>> EnemyObjectPools;

    public static bool IsInitialised;
    public static void Init()
    {
        if (!IsInitialised)
        {
            EnemyPrefabs = new Dictionary<int, GameObject>();
            EnemyObjectPools = new Dictionary<int, Queue<Enemy>>();
            EnemiesInGame = new List<Enemy>();

            EnemySummonData[] Enemies = Resources.LoadAll<EnemySummonData>("Enemies");
            

            foreach (EnemySummonData enemy in Enemies)
            {
                EnemyPrefabs.Add(enemy.EnemyID, enemy.EnemyPrefab);
                EnemyObjectPools.Add(enemy.EnemyID, new Queue<Enemy>());
            }

            IsInitialised = true;
        }
        else
        {
            Debug.Log("ALREADY INITIALISED");
        }
        
    }

    public static Enemy SummonEnemy(int EnemyID)
    {
        Enemy SummonedEnemy = null;

        if (EnemyPrefabs.ContainsKey(EnemyID)) //Check if an enemy with that ID exists
        {
            Queue<Enemy> ReferencedQueue = EnemyObjectPools[EnemyID];
            if (ReferencedQueue.Count > 0) //Check there are enemies left in queue
            {
                //Dequue Enemy and initialise
                SummonedEnemy = ReferencedQueue.Dequeue();
                SummonedEnemy.Init();
            }
            else
            {
                //Instantiate new instance of enemy and initialise
                GameObject NewEnemy = Instantiate(EnemyPrefabs[EnemyID], Vector3.zero, Quaternion.identity);
                SummonedEnemy = NewEnemy.GetComponent<Enemy>();
                SummonedEnemy.Init();
            }
        }
        else
        {
            Debug.Log($"Enemy with ID of {EnemyID} does not exist");
            return null;
        }

        return SummonedEnemy;
    }
      
}
