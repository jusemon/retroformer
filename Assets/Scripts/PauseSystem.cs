using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PauseSystem : MonoBehaviour
{
    public GameObject pausePanel;

    public Button[] buttonsWithContinue = new Button[0];

    public Button[] buttonsWithPause = new Button[0];

    public Button[] buttonsWithSave = new Button[0];

    public Button[] buttonsWithExit = new Button[0];

    public Transform player;

    private AudioSource[] audioSources;

    void Start()
    {
        pausePanel.SetActive(false);

        foreach (var button in buttonsWithContinue)
        {
            button.onClick.AddListener(ContinueGame);
        }

        foreach (var button in buttonsWithPause)
        {
            button.onClick.AddListener(PauseGame);
        }

        foreach (var button in buttonsWithSave)
        {
            button.onClick.AddListener(SaveGame);
        }

        foreach (var button in buttonsWithExit)
        {
            button.onClick.AddListener(ExitGame);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel"))
        {
            if (!pausePanel.activeInHierarchy)
            {
                PauseGame();
            }
            else if (pausePanel.activeInHierarchy)
            {
                ContinueGame();
            }
        }
    }

    public void PauseGame()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        Time.timeScale = 0;
        pausePanel.SetActive(true);
        audioSources = FindObjectsOfType<AudioSource>().Where(aus => aus.isPlaying).ToArray();
        foreach (var item in audioSources)
        {
            item.Pause();
        }
        // Disable scripts that still work while timescale is set to 0
    }

    public void ContinueGame()
    {
        if (Time.timeScale == 1)
        {
            return;
        }

        Time.timeScale = 1;
        pausePanel.SetActive(false);
        foreach (var item in audioSources)
        {
            item.UnPause();
        }
    }

    private void SaveGame()
    {
        var slot = SaveLoadSystem.GetCurrentSlot();
        var saveData = SaveLoadSystem.Get(slot);
        saveData.positionX = player.position.x;
        saveData.positionY = player.position.y;
        SaveLoadSystem.Save(slot, saveData);

        FindObjectOfType<ModalScreenSystem>().Show("Game Saved!", "Close", () => { });
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
