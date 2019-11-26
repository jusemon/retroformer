using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float boundLeft = 0;

    public float boundRight = 100;

    private void Start()
    {
        if (!player)
        {
            Debug.LogError("Player not assigned to CameraFollow");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(player.position.x, boundLeft, boundRight), transform.position.y, transform.position.z);
    }
}
