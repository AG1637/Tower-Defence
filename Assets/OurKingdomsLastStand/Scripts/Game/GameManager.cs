using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static List<TowerBehaviour> TowersInGame;

    public bool paused;

    void Start()
    {
        instance = this; //used to reference GameLoopManager from other scripts
        TowersInGame = new List<TowerBehaviour>();
    }
}
