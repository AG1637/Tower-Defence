using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenClosePanels : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject levelSelectPanel;
    [SerializeField] GameObject creditsPanel;

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }
    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenLevelSelectPanel()
    {
        levelSelectPanel.SetActive(true);
    }
    public void CloseLevelSelectPanel()
    {
        levelSelectPanel.SetActive(false);
    }
    public void OpenCreditsPanel()
    {
        creditsPanel.SetActive(true);
    }
    public void CloseCreditsPanel()
    {
        creditsPanel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
