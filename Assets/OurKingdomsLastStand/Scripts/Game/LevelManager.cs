using Unity.AppUI.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int currentLevel;

    [Header ("Level Buttons")]
    public GameObject Level1Button;
    public GameObject Level2Button;
    public GameObject Level3Button;
    public GameObject Level4Button;
    public GameObject Level5Button;
    public GameObject Level6Button;
    public GameObject Level7Button;
    public GameObject Level8Button;
    public GameObject Level9Button;

    private void Start()
    {
        //Level1Button.GetComponent<Button>() = false;
    }
    public void Update()
    {
        
    }

    public void LoadLevel1()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadSceneAsync(2);
    }

}
