using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class WaterZone : MonoBehaviour
{
    [SerializeField]
    private float timeDelay = 1.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player enters within the Water Zone, then restart the scene 
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(restartScene());
        }
    }

    private IEnumerator restartScene()
    {
        // Wait timeDelay 
        yield return new WaitForSeconds(timeDelay);

        Time.timeScale = 0;

        // Show a gameover message
        FindObjectOfType<ModalScreenController>().Show("GAME OVER", "Try again?", () =>
        {
            // Restart Scene 
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
}
