using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BabyMovement : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public float maxSpeedX;


    Rigidbody2D rb;
    Animator anim;
    private bool isRunning;


    // Use this for initialization
    void Start()
    {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.anim = this.GetComponent<Animator>();
    }


    void Update()
    {
        // Horizontal input
        float moveHorizontal = Input.GetAxis("Horizontal");

        // Vertical input
        // float moveVertical = Input.GetAxis("Vertical");


        // Move horizontally, but only if we're below max running speed.
        if (this.rb.velocity.x < maxSpeedX)
        {
			Vector2 horizontalMovementForce = new Vector2(moveHorizontal * speed, 0);
            this.rb.AddForce(horizontalMovementForce);
		}


        // Should we animate horizontal movement?
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if (!isRunning)
            {
                this.anim.SetTrigger("shouldRun");
                this.isRunning = true;
            }
        }


        // Should we jump? (Second condition ensures we're not in midair trying to double jump)
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(this.rb.velocity.y) < 0.00001f)
        {
            this.anim.SetTrigger("shouldJump");
            Vector2 jumpForce = new Vector2(0, this.jumpPower);
            this.rb.AddForce(jumpForce);
        }


        // Should we stop moving horizontally?
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            this.isRunning = false;
            this.anim.SetTrigger("shouldIdle");
            this.rb.velocity = new Vector2(0f, this.rb.velocity.y);
            //this.rb.angularVelocity = 0f;
        }

    }


    void LateUpdate()
    {


    }


    void FixedUpdate()
    {

    }
}
