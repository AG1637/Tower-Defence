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
    public static List<TowerBehaviour> TowersInGame;

    public bool paused;

    void Start()
    {
        instance = this; //used to reference GameLoopManager from other scripts
        TowersInGame = new List<TowerBehaviour>();
    }
}