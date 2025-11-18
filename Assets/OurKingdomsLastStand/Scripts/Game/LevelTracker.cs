using UnityEngine;

public class LevelTracker : MonoBehaviour
{
    public static LevelTracker instance;
    public int currentLevel = 1;

    void Awake()
    {
        if (instance) { Destroy(this.gameObject); }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

}
