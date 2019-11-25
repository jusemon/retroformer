using UnityEngine;

public class PlayerController : MonoBehaviour
{
#if UNITY_STANDALONE || UNITY_EDITOR
    MovementComponent movementComponent;

    private void Awake()
    {
        movementComponent = GetComponent<MovementComponent>();

        //Disable this script in case the Movement Component is not found, and leave an error message. 
        if (movementComponent == null)
        {
            Debug.LogError("Missing MovementComponent on " + gameObject.name + " to run PlayerController. Please add one.");
            Destroy(this);
        }
    }

    void FixedUpdate()
    {
        //Send input to the Movement Component in order to move the character

        movementComponent.MoveCharacter(Input.GetAxis("Horizontal"));
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            movementComponent.Jump();
        }
    }
#endif
}
