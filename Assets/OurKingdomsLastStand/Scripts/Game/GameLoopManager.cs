using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UIElements;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager instance;
    public static Vector3[] nodePositions;
    public static float[] nodeDistance;
    public static List<TowerBehaviour> TowersInGame;

    private static Queue<Enemy> EnemiesToRemove;
    private static Queue<int> EnemyIDsToSummon;

    public Transform NodeParent;
    public bool loopShouldEnd;
    public bool paused;

    void Start()
    {
        instance = this; //used to reference GameLoopManager from other scripts
        TowersInGame = new List<TowerBehaviour>();
        EnemyIDsToSummon = new Queue<int>();
        EnemiesToRemove = new Queue<Enemy>();
        //EntitySummoning.Init();

        nodePositions = new Vector3[NodeParent.childCount];
                for(int i =0; i < nodePositions.Length; i++)
        {
            nodePositions[i] = NodeParent.GetChild(i).position;
        }

        nodeDistance = new float[nodePositions.Length - 1];
        for (int i = 0; i < nodeDistance.Length; i++)
        {
            nodeDistance[i] = Vector3.Distance(nodePositions[i], nodePositions[i + 1]);
        }

        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop() //iteration
    {
        while (loopShouldEnd == false)
        {
            /*
            //Spawn Enemies
            if(EnemyIDsToSummon.Count > 0)
            {
                for(int i = 0; i < EnemyIDsToSummon.Count; i++)
                {
                    EntitySummoning.SummonEnemy(EnemyIDsToSummon.Dequeue());
                }
            }

            //Remove Enemies

            if(EnemiesToRemove.Count > 0)
            {
                for (int i = 0; i < EnemiesToRemove.Count; i++)
                {
                    EntitySummoning.RemoveEnemy(EnemiesToRemove.Dequeue());
                }
            }

            //Move Enemies

            NativeArray<Vector3> NodesToUse = new NativeArray<Vector3>(nodePositions, Allocator.TempJob);
            NativeArray<float> EnemySpeeds = new NativeArray<float>(EntitySummoning.EnemiesInGame.Count, Allocator.TempJob);
            NativeArray<int> NodeIndices = new NativeArray<int>(EntitySummoning.EnemiesInGame.Count, Allocator.TempJob);
            TransformAccessArray EnemyAccess = new TransformAccessArray(EntitySummoning.EnemiesInGameTransform.ToArray(), 2);

            for (int i = 0; i < EntitySummoning.EnemiesInGame.Count; i++)
            {
                EnemySpeeds[i] = EntitySummoning.EnemiesInGame[i].speed;
                NodeIndices[i] = EntitySummoning.EnemiesInGame[i].nodeIndex;
            }

            MoveEnemiesJob MoveJob = new MoveEnemiesJob
            {
                NodePositions = NodesToUse,
                EnemySpeed = EnemySpeeds,
                NodeIndex = NodeIndices,
                deltaTime = Time.deltaTime
            };

            JobHandle MoveJobHandle = MoveJob.Schedule(EnemyAccess);
            MoveJobHandle.Complete();

            for (int i = 0; i < EntitySummoning.EnemiesInGame.Count; i++)
            {
                EntitySummoning.EnemiesInGame[i].nodeIndex = NodeIndices[i];

                if(EntitySummoning.EnemiesInGame[i].nodeIndex == nodePositions.Length)
                {
                    EnqueueEnemyToRemove(EntitySummoning.EnemiesInGame[i]);
                }
            }
            
            EnemySpeeds.Dispose();
            NodeIndices.Dispose();
            EnemyAccess.Dispose();
            NodesToUse.Dispose();
            
            Tick Towers

            foreach(TowerBehaviour tower in TowersInGame)
            {
                tower.Target = TowerTargeting.GetTarget(tower, TowerTargeting.TargetType.First);
                tower.Tick();
            }
            */

           yield return null;
        }
    }

    public static void EnqueueEnemyIDToSummon(int ID)
    {
        EnemyIDsToSummon.Enqueue(ID);
    }

    public static void EnqueueEnemyToRemove(Enemy EnemyToRemove)
    {
        EnemiesToRemove.Enqueue(EnemyToRemove);
    }
}

public struct MoveEnemiesJob : IJobParallelForTransform
{
    [NativeDisableParallelForRestriction]
    public NativeArray<Vector3> NodePositions;

    [NativeDisableParallelForRestriction]
    public NativeArray<float> EnemySpeed;

    [NativeDisableParallelForRestriction]
    public NativeArray<int> NodeIndex;

    public float deltaTime;

    public void Execute(int index, TransformAccess transform)
    {
        if (NodeIndex[index] < NodePositions.Length)
        {
            Vector3 PositionToMoveTo = NodePositions[NodeIndex[index]];
            transform.position = Vector3.MoveTowards(transform.position, PositionToMoveTo, EnemySpeed[index] * deltaTime);

            if (transform.position == PositionToMoveTo)
            {
                NodeIndex[index]++;
            }
        }
    }
}