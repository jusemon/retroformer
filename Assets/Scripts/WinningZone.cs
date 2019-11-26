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
        // TODO: Show a winning message (left as exercise) 

        // Wait timeDelay 
        yield return new WaitForSeconds(timeDelay);


        // Load the level named as in the nextLevelToLoad variable. 
        SceneManager.LoadScene(nextLevelToLoad);
    }


}
