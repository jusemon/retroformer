using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool inMainMenu = false;

    private void Start()
    {
        if (inMainMenu)
        {
            return;
        }

        var saveData = SaveLoadSystem.Get(SaveLoadSystem.GetCurrentSlot());

        gameObject.transform.SetPositionAndRotation(new Vector3(
            saveData.positionX == 0 ? gameObject.transform.position.x : saveData.positionX,
            saveData.positionY == 0 ? gameObject.transform.position.y : saveData.positionY,
            gameObject.transform.position.z
        ), Quaternion.identity);
    }

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
}
