using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EntitySummoning : MonoBehaviour
{
    public static List<Enemy> EnemiesInGame;
    public static List<Transform> EnemiesInGameTransform;
    public static Dictionary<int, GameObject> EnemyPrefabs;
    public static Dictionary<int, Queue<Enemy>> EnemyObjectPools;

    public static bool IsInitialised;
    public static void Init()
    {
        if (!IsInitialised)
        {
            EnemyPrefabs = new Dictionary<int, GameObject>();
            EnemyObjectPools = new Dictionary<int, Queue<Enemy>>();
            EnemiesInGameTransform = new List<Transform>();
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
                //Dequeue Enemy and initialise
                SummonedEnemy = ReferencedQueue.Dequeue();
                SummonedEnemy.Init();
                SummonedEnemy.gameObject.SetActive(true); //reactivate enemies when they are summoned
            }
            else
            {
                //Instantiate new instance of enemy and initialise
                GameObject NewEnemy = Instantiate(EnemyPrefabs[EnemyID], GameLoopManager.NodePositions[0], Quaternion.identity);
                SummonedEnemy = NewEnemy.GetComponent<Enemy>();
                SummonedEnemy.Init();
            }
        }
        else
        {
            Debug.Log($"Enemy with ID of {EnemyID} does not exist");
            return null;
        }

        EnemiesInGameTransform.Add(SummonedEnemy.transform);
        EnemiesInGame.Add(SummonedEnemy); //Adds every new enemy that is spawned into list for current enemies
        SummonedEnemy.ID = EnemyID;
        return SummonedEnemy;
    }

    public static void RemoveEnemy(Enemy EnemyToRemove)
    {
        EnemyObjectPools[EnemyToRemove.ID].Enqueue(EnemyToRemove);
        EnemyToRemove.gameObject.SetActive(false);
        EnemiesInGameTransform.Remove(EnemyToRemove.transform);
        EnemiesInGame.Remove(EnemyToRemove);
    }
      
}
