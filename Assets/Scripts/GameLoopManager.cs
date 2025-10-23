using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.Jobs;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UIElements;

public class GameLoopManager : MonoBehaviour
{
    public static Vector3[] NodePositions;
    private static Queue<Enemy> EnemiesToRemove;
    private static Queue<int> EnemyIDsToSummon;
    public Transform NodeParent;
    public bool LoopShouldEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyIDsToSummon = new Queue<int>();
        EnemiesToRemove = new Queue<Enemy>();
        EntitySummoning.Init();

        NodePositions = new Vector3[NodeParent.childCount];
                for(int i =0; 1 < NodePositions.Length; i++)
        {
            NodePositions[i] = NodeParent.GetChild(i).position;
        }
        
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

            //Remove Enemies

            if(EnemiesToRemove.Count > 0)
            {
                for (int i = 0; i < EnemiesToRemove.Count; i++)
                {
                    EntitySummoning.RemoveEnemy(EnemiesToRemove.Dequeue());
                }
            }

            //Move Enemies

            NativeArray<Vector3> NodesToUse = new NativeArray<Vector3>(NodePositions, Allocator.TempJob);
            NativeArray<float> EnemySpeeds = new NativeArray<float>(EntitySummoning.EnemiesInGame.Count, Allocator.TempJob);
            NativeArray<int> NodeIndices = new NativeArray<int>(EntitySummoning.EnemiesInGame.Count, Allocator.TempJob);
            TransformAccessArray EnemyAccess = new TransformAccessArray(EntitySummoning.EnemiesInGameTransform.ToArray(), 2);

            for (int i = 0; i < EntitySummoning.EnemiesInGame.Count; i++)
            {
                EnemySpeeds[i] = EntitySummoning.EnemiesInGame[i].Speed;
                NodeIndices[i] = EntitySummoning.EnemiesInGame[i].NodeIndex;
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
                EntitySummoning.EnemiesInGame[i].NodeIndex = NodeIndices[i];

                if(EntitySummoning.EnemiesInGame[i].NodeIndex == NodePositions.Length)
                {
                    EnqueueEnemyToRemove(EntitySummoning.EnemiesInGame[i]);
                }
            }
            
            EnemySpeeds.Dispose();
            NodeIndices.Dispose();
            EnemyAccess.Dispose();
            NodesToUse.Dispose();

            //Spawn Towers

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