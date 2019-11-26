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
        // TODO: Show a gameover message (left as exercise)

        // Wait timeDelay 
        yield return new WaitForSeconds(timeDelay);
        // Restart Scene 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
