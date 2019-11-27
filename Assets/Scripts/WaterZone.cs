using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class WaterZone : MonoBehaviour
{
    [SerializeField]
    private float timeDelay = 1.0f;

    private float collisionEnter = 0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collisionEnter -= Time.deltaTime;
            if (collisionEnter <= 0)
            {
                restartScene();
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player enters within the Water Zone, then restart the scene 
        if (collision.CompareTag("Player"))
        {
            collisionEnter = timeDelay;
        }
    }

    private void restartScene()
    {
        Time.timeScale = 0;

        // Show a gameover message
        FindObjectOfType<ModalScreenSystem>().Show("GAME OVER", "Try again?", () =>
        {
            // Restart Scene 
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        Destroy(this);
    }
}
