using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
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
        Level2Button.GetComponent<Button>().interactable = FindFirstObjectByType<LevelTracker>().currentLevel >= 1;
        Level3Button.GetComponent<Button>().interactable = false;
        Level4Button.GetComponent<Button>().interactable = false;
        Level5Button.GetComponent<Button>().interactable = false;
        Level6Button.GetComponent<Button>().interactable = false;
        Level7Button.GetComponent<Button>().interactable = false;
        Level8Button.GetComponent<Button>().interactable = false; 
        Level9Button.GetComponent<Button>().interactable = false;
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene(2);
    }

}
