using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class WinningZone : MonoBehaviour
{
    [SerializeField]
    private float timeDelay = 1.0f;

    [SerializeField]
    private string nextLevelToLoad = "Level2";

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

        Time.timeScale = 0;

        // Show a winning message
        FindObjectOfType<ModalScreenSystem>().Show("LEVEL PASSED", "Next level", () =>
        {
            Time.timeScale = 1;
            // Load the level named as in the nextLevelToLoad variable. 
            SceneManager.LoadScene(nextLevelToLoad);
        });
    }
}
