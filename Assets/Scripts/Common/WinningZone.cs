using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class WinningZone : MonoBehaviour
{
    [SerializeField]
    private float timeDelay = 1.0f;

    [SerializeField]
    private int nextLevelToLoad = 2;

    [SerializeField]
    private bool hasNextLevel = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player enters within the Water Zone, then restart the scene 
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(loadNextLevel());
        }
    }


    private IEnumerator loadNextLevel()
    {
        // Wait timeDelay 
        yield return new WaitForSeconds(timeDelay);

        // Pause Game
        Time.timeScale = 0;

        // Save Game Progress
        var slotKey = SaveLoadSystem.GetCurrentSlot();
        var saveData = SaveLoadSystem.Get(slotKey);
        saveData.positionX = 0;
        saveData.positionY = 0;
        saveData.timeLimit = 0;

        if (hasNextLevel)
        {
            saveData.level = nextLevelToLoad;
            SaveLoadSystem.Save(slotKey, saveData);

            // Show a winning message
            FindObjectOfType<ModalScreenSystem>().Show("LEVEL PASSED", "Next level", () =>
            {
                Time.timeScale = 1;
                // Load the level named as in the nextLevelToLoad variable. 
                SceneManager.LoadScene($"Level{nextLevelToLoad.ToString()}");
            });
        }
        else {
            SaveLoadSystem.Save(slotKey, saveData);

            // Show a winning message
            FindObjectOfType<ModalScreenSystem>().Show("GAME FINISHED", "Exit", () =>
            {
                Time.timeScale = 1;
                // Load the level named as in the nextLevelToLoad variable. 

                SceneManager.LoadScene("StartScreen");
            });
        }
    }
}
