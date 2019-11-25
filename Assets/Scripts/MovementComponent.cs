using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class MovementComponent : MonoBehaviour
{
    private Animator anim;

    private Rigidbody2D rb2d;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float moveForce = 360f;

    [SerializeField]
    private float maxSpeed = 5f;

    [SerializeField]
    private float jumpForce = 1000f;

    [SerializeField]
    private Transform groundCheck;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (groundCheck == null)
        {
            Debug.LogError("Ground Check missing from the MovementComponent, please set one.");
            Destroy(this);
        }
    }

    public void MoveCharacter(float normalisedSpeed)
    {
        //If the max velocity is not reached, then... 
        if (rb2d.velocity.x * normalisedSpeed < maxSpeed)
        {
            //... apply a force to the character 
            rb2d.AddForce(Vector2.right * normalisedSpeed * moveForce);
        }

        float clampVelocityX = Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed);
        rb2d.velocity = new Vector2(clampVelocityX, rb2d.velocity.y);
    }

    public void Jump()
    {
        // Get the index for the Ground layer mask. 
        int layerMaskIndex = LayerMask.NameToLayer("Ground");

        // Calculate the layer mask value that can be used in the following 
        // Physics2D.Linecast() method call. We use a bitwise left shift 
        // operation to find the correct value which is 256 because we 
        // use the 8th User Layer in our example: 1 << 8 = 256. 
        int groundCheckLayerMask = 1 << layerMaskIndex;

        //Check if the character can jump 
        if (Physics2D.Linecast(transform.position, groundCheck.position, groundCheckLayerMask))
        {
            if (rb2d.velocity.y <= 0)
            {
                //Perform the jump 
                rb2d.AddForce(new Vector2(0f, jumpForce));
            }
        }
    }

    void Update()
    {
        // Set the Speed parameter in the Animation State Machine 
        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));

        // Set the Jump parameter in the Animation State Machine 
        anim.SetBool("Jump", rb2d.velocity.y != 0);

        //Check in which direction the sprite should face and flip accordingly 
        if (rb2d.velocity.x != 0)
        {
            spriteRenderer.flipX = rb2d.velocity.x < 0;
        }
    }
}
